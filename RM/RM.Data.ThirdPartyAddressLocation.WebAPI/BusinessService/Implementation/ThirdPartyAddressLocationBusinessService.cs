using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO.FileProcessing;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Utils;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService
{
    /// <summary>
    /// Class definition for the Third Party business Service members
    /// </summary>
    public class ThirdPartyAddressLocationBusinessService : IThirdPartyAddressLocationBusinessService
    {
        private IAddressLocationDataService addressLocationDataService = default(IAddressLocationDataService);
        private IThirdPartyAddressLocationIntegrationService thirdPartyAddressLocationIntegrationService = default(IThirdPartyAddressLocationIntegrationService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ThirdPartyAddressLocationBusinessService(
                                                        IAddressLocationDataService addressLocationDataService,
                                                        IThirdPartyAddressLocationIntegrationService thirdPartyAddressLocationIntegrationService,
                                                        ILoggingHelper loggingHelper)
        {
            // Store injected dependencies
            this.addressLocationDataService = addressLocationDataService;
            this.thirdPartyAddressLocationIntegrationService = thirdPartyAddressLocationIntegrationService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>Address Location DTO</returns>
        public async Task<object> GetAddressLocationByUDPRNJson(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAddressLocationByUDPRNJson"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetAddressLocationByUDPRNJson);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                AddressLocationDataDTO addressLocationDataDto = await this.addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
                Mapper.Initialize(cfg => cfg.CreateMap<AddressLocationDataDTO, AddressLocationDTO>());
                AddressLocationDTO addressLocationDto = Mapper.Map<AddressLocationDataDTO, AddressLocationDTO>(addressLocationDataDto);
                var getAddressLocationJsonData = GetAddressLocationJsonData(addressLocationDto);
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return getAddressLocationJsonData;
            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<AddressLocationDTO> GetAddressLocationByUDPRN(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAddressLocationByUDPRN"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetAddressLocationByUDPRN);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                var addressLocationDataDto = await addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AddressLocationDataDTO, AddressLocationDTO>().MaxDepth(1);
                });
                Mapper.Configuration.CreateMapper();

                AddressLocationDTO addressLocationDto = Mapper.Map<AddressLocationDataDTO, AddressLocationDTO>(addressLocationDataDto);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return addressLocationDto;
            }
        }

        #region Save USR Details to Database

        // TODO : Add method when ready
        // To be implemented in parallel
        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos">List of Address Locations</param>
        /// <returns>Task</returns>
        public async Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            int fileUdprn;
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveUSRDetails"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(SaveUSRDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                            {
                                ThirdPartyAddressLocationConstants.TASKNOTIFICATION,
                                ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER,
                                ThirdPartyAddressLocationConstants.DeliveryPointUseIndicator,
                                ThirdPartyAddressLocationConstants.APPROXLOCATION,
                                ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                                ReferenceDataCategoryNames.NetworkNodeType
                            };

                // Get all the reference data Guids required for the below
                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, ThirdPartyAddressLocationConstants.TASKNOTIFICATION, ThirdPartyAddressLocationConstants.TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER, ThirdPartyAddressLocationConstants.EXTERNAL);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ThirdPartyAddressLocationConstants.OperationalStatusGUIDLive, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, ThirdPartyAddressLocationConstants.NetworkNodeTypeRMGServiceNode, true);
                Guid approxLocation = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ThirdPartyAddressLocationConstants.APPROXLOCATION, true);
                Guid notificationTypeId_GUID = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(ThirdPartyAddressLocationConstants.USRCATEGORY, ThirdPartyAddressLocationConstants.USRREFERENCEDATANAME);

                foreach (AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO in addressLocationUsrpostdtos)
                {
                    // Get the udprn id for each USR record.
                    fileUdprn = (int)addressLocationUSRPOSTDTO.UDPRN;

                    if (!await addressLocationDataService.AddressLocationExists(fileUdprn))
                    {
                        DbGeometry spatialLocationXY = GetSpatialLocation(addressLocationUSRPOSTDTO);

                        AddressLocationDataDTO newAddressLocationDTO = new AddressLocationDataDTO()
                        {
                            ID = Guid.NewGuid(),
                            UDPRN = (int)addressLocationUSRPOSTDTO.UDPRN,
                            LocationXY = spatialLocationXY,
                            Lattitude = (decimal)addressLocationUSRPOSTDTO.Latitude,
                            Longitude = (decimal)addressLocationUSRPOSTDTO.Longitude
                        };

                        // Save the address location data record to the database.
                        await addressLocationDataService.SaveNewAddressLocation(newAddressLocationDTO);

                        PostalAddressDataDTO postalAddressDataDTO = await addressLocationDataService.GetPostalAddressData((int)fileUdprn);

                        // Check if the delivery point exists
                        if (postalAddressDataDTO.DeliveryPoints != null && postalAddressDataDTO.DeliveryPoints.Count > 0)
                        {
                            DeliveryPointDTO deliveryPointDTO = await thirdPartyAddressLocationIntegrationService.GetDeliveryPointByPostalAddress(postalAddressDataDTO.ID);

                            // Check if the existing delivery point has an approx location.
                            if (deliveryPointDTO.OperationalStatus_GUID == approxLocation)
                            {
                                deliveryPointDTO.LocationXY = spatialLocationXY;
                                deliveryPointDTO.LocationProvider_GUID = locationProviderId;
                                deliveryPointDTO.UDPRN = fileUdprn;
                                deliveryPointDTO.OperationalStatus_GUID = operationalStatusGUIDLive;
                                deliveryPointDTO.NetworkNodeType_GUID = networkNodeTypeRMGServiceNode;

                                // Update the location details for the delivery point
                                await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointById(deliveryPointDTO);

                                // Check if a notification exists for the UDPRN.
                                if (await addressLocationDataService.CheckIfNotificationExists(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION))
                                {
                                    // update the notification if it exists.
                                    await thirdPartyAddressLocationIntegrationService.UpdateNotificationByUDPRN(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION, ThirdPartyAddressLocationConstants.NOTIFICATIONCLOSED);
                                }
                            }
                            else
                            {
                                // Calculates the straight line distance between the existing delivery
                                // point and the new delivery point.
                                var straightLineDistance = GetDeliveryPointDistance(deliveryPointDTO, spatialLocationXY);

                                // Check if the new point is within the tolerance limit
                                if (straightLineDistance <= ThirdPartyAddressLocationConstants.TOLERANCEDISTANCEINMETERS)
                                {
                                    deliveryPointDTO.LocationXY = spatialLocationXY;
                                    deliveryPointDTO.LocationProvider_GUID = locationProviderId;
                                    deliveryPointDTO.UDPRN = fileUdprn;
                                    deliveryPointDTO.OperationalStatus_GUID = operationalStatusGUIDLive;
                                    deliveryPointDTO.NetworkNodeType_GUID = networkNodeTypeRMGServiceNode;

                                    // Update the delivery point location directly in case it is within
                                    // the tolerance limits.
                                    await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointById(deliveryPointDTO);

                                    // Check if the notification exists for the given UDPRN.
                                    if (await addressLocationDataService.CheckIfNotificationExists(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION))
                                    {
                                        // update the notification if it exists.
                                        await thirdPartyAddressLocationIntegrationService.UpdateNotificationByUDPRN(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION, ThirdPartyAddressLocationConstants.NOTIFICATIONCLOSED);
                                    }
                                }
                                else
                                {
                                    // Get the Postcode Sector by UDPRN
                                    PostCodeSectorDTO postCodeSectorDTO =
                                        await thirdPartyAddressLocationIntegrationService.GetPostCodeSectorByUDPRN(fileUdprn);

                                    PostalAddressDataDTO postalAddressDTO = await addressLocationDataService.GetPostalAddressData(fileUdprn);

                                    NotificationDTO notificationDO = new NotificationDTO
                                    {
                                        ID = Guid.NewGuid(),
                                        NotificationType_GUID = notificationTypeId_GUID,
                                        NotificationDueDate = DateTime.UtcNow.AddHours(ThirdPartyAddressLocationConstants.NOTIFICATIONDUE),
                                        NotificationSource = ThirdPartyAddressLocationConstants.USRNOTIFICATIONSOURCE,
                                        Notification_Heading = ThirdPartyAddressLocationConstants.USRACTION,
                                        Notification_Message = AddressFields(postalAddressDTO),
                                        PostcodeDistrict = (postCodeSectorDTO == null || postCodeSectorDTO.District == null)
                                            ? string.Empty
                                            : postCodeSectorDTO.District,
                                        PostcodeSector = (postCodeSectorDTO == null || postCodeSectorDTO.Sector == null)
                                            ? string.Empty
                                            : postCodeSectorDTO.Sector,
                                        NotificationActionLink = string.Format(ThirdPartyAddressLocationConstants.USRNOTIFICATIONLINK, fileUdprn)
                                    };

                                    // Insert the new notification.
                                    await thirdPartyAddressLocationIntegrationService.AddNewNotification(notificationDO);
                                }
                            }
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// This method is used to fetch GeoJson data for Address Location.
        /// </summary>
        /// <param name="addressLocationDto">Address Location DTO</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private object GetAddressLocationJsonData(AddressLocationDTO addressLocationDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAddressLocationJsonData"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetAddressLocationJsonData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                var addressLocationGeoJson = new GeoJson
                {
                    features = new List<Feature>()
                };

                if (addressLocationDto.LocationXY != null)
                {
                    SqlGeometry addressLocationSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addressLocationDto.LocationXY.AsBinary()), ThirdPartyAddressLocationConstants.BNGCOORDINATESYSTEM);

                    var feature = new Feature
                    {
                        id = addressLocationDto.ID.ToString(),
                        properties = new Dictionary<string, JToken>
                    {
                        { ThirdPartyAddressLocationConstants.UDPRN, addressLocationDto.UDPRN },
                        { ThirdPartyAddressLocationConstants.Latitude, addressLocationDto.Lattitude },
                        { ThirdPartyAddressLocationConstants.Longitude, addressLocationDto.Longitude },
                    },
                        geometry = new Geometry
                        {
                            coordinates = new double[] { addressLocationSqlGeometry.STX.Value, addressLocationSqlGeometry.STY.Value }
                        }
                    };

                    addressLocationGeoJson.features.Add(feature);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return addressLocationGeoJson;
            }
        }

        #endregion Save USR Details to Database

        public async Task<List<AddressLocationDTO>> GetAddressLocationsByUDPRN(List<int> udprns)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAddressLocationsbyUDPRN"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetAddressLocationByUDPRN);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                var addressLocationDataDtos = await addressLocationDataService.GetAddressLocationsByUDPRN(udprns);
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AddressLocationDataDTO, AddressLocationDTO>().MaxDepth(1);
                });
                Mapper.Configuration.CreateMapper();

                List<AddressLocationDTO> addressLocationDtos = Mapper.Map<List<AddressLocationDataDTO>, List<AddressLocationDTO>>(addressLocationDataDtos);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return addressLocationDtos;
            }
        }

        #region Calculate Spatial Location

        /// <summary>
        /// Get the geometry equivalent of the X-Y co-ordinate of address location
        /// </summary>
        /// <param name="addressLocationUSRPOSTDTO">AddressLocationUSRPOSTDTO object</param>
        /// <returns>DbGeometry object</returns>
        private DbGeometry GetSpatialLocation(AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetSpatialLocation"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetSpatialLocation);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                string sbLocationXY = string.Format(
                ThirdPartyAddressLocationConstants.USRGEOMETRYPOINT,
                Convert.ToString(addressLocationUSRPOSTDTO.XCoordinate),
                Convert.ToString(addressLocationUSRPOSTDTO.YCoordinate));

                // Convert the location from string type to geometry type
                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), ThirdPartyAddressLocationConstants.BNGCOORDINATESYSTEM);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return spatialLocationXY;
            }
        }

        #endregion Calculate Spatial Location

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">USR create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.AddressFields"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(AddressFields);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                string message = ThirdPartyAddressLocationConstants.USRNOTIFICATIONBODYPREFIX +
                        objPostalAddress.OrganisationName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DepartmentName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.BuildingName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.BuildingNumber + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.SubBuildingName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.Thoroughfare + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DependentThoroughfare + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DependentLocality + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DoubleDependentLocality + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.PostTown + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.Postcode;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return message;
            }
        }

        /// <summary>
        /// Calculte distance between two points
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPoint DTO</param>
        /// <param name="newPoint">newPoint as DbGeometry</param>
        /// <returns>distance as double</returns>
        private double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetDeliveryPointDistance"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetDeliveryPointDistance);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                double? distance = 0;
                if (deliveryPointDTO != null)
                {
                    distance = deliveryPointDTO.LocationXY.Distance(newPoint);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return distance;
            }
        }

        /// <summary>
        /// Get Reference Data from
        /// </summary>
        /// <param name="categoryNamesSimpleLists"></param>
        /// <param name="categoryName"></param>
        /// <param name="referenceDataValue"></param>
        /// <param name="isWithSpace"></param>
        /// <returns></returns>
        private Guid GetReferenceData(List<string> categoryNamesSimpleLists, string categoryName, string referenceDataValue, bool isWithSpace = false)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                var referenceDataCategoryList = thirdPartyAddressLocationIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                Guid referenceDataGuid = Guid.Empty;
                if (isWithSpace)
                {
                    referenceDataGuid = referenceDataCategoryList
                                       .Where(list => list.CategoryName.Replace(" ", string.Empty) == categoryName)
                                       .SelectMany(list => list.ReferenceDatas)
                                       .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                       .Select(s => s.ID).SingleOrDefault();
                }
                else
                {
                    referenceDataGuid = referenceDataCategoryList
                                    .Where(list => list.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(list => list.ReferenceDatas)
                                    .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.ID).SingleOrDefault();
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return referenceDataGuid;
            }
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">USR create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressField(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.AddressFields"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(AddressFields);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                string message = ThirdPartyAddressLocationConstants.USRNOTIFICATIONBODYPREFIX +
                        objPostalAddress.OrganisationName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DepartmentName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.BuildingName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.BuildingNumber + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.SubBuildingName + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.Thoroughfare + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DependentThoroughfare + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DependentLocality + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.DoubleDependentLocality + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.PostTown + ThirdPartyAddressLocationConstants.Comma +
                        objPostalAddress.Postcode;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return message;
            }
        }

        /// <summary>
        /// Method to delete the list of Third Party Address Location data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos">List of Address Locations</param>
        /// <returns>Task</returns>
        public async Task DeleteUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            int fileUdprn;
            using (loggingHelper.RMTraceManager.StartTrace("Business.DeleteUSRDetails"))
            {
                string methodName = typeof(ThirdPartyAddressLocationBusinessService) + "." + nameof(DeleteUSRDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                            {
                                ThirdPartyAddressLocationConstants.TASKNOTIFICATION,
                                ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER,
                                ThirdPartyAddressLocationConstants.DeliveryPointUseIndicator,
                                ThirdPartyAddressLocationConstants.APPROXLOCATION,
                                ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                                ReferenceDataCategoryNames.NetworkNodeType
                            };

                // Get all the reference data Guids required for the below
                Guid locationProviderInternalId = GetReferenceData(categoryNamesSimpleLists, ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER, ThirdPartyAddressLocationConstants.INTERNAL);
                Guid realLocation = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ThirdPartyAddressLocationConstants.APPROXLOCATION, true);
                Guid notificationTypeId_GUID = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(ThirdPartyAddressLocationConstants.USRCATEGORY, ThirdPartyAddressLocationConstants.USRREFERENCEDATANAME);

                foreach (AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO in addressLocationUsrpostdtos)
                {
                    // Get the udprn id for each USR record.
                    fileUdprn = (int)addressLocationUSRPOSTDTO.UDPRN;
                    var addressLocation = addressLocationDataService.GetAddressLocationByUDPRN(fileUdprn).Result;

                    if (addressLocation != null)
                    {
                        // Delete the address location data record to the database.
                        await addressLocationDataService.DeleteAddressLocation(addressLocation);

                        PostalAddressDataDTO postalAddressDataDTO = await addressLocationDataService.GetPostalAddressData((int)fileUdprn);

                        // Check if the delivery point exists
                        if (postalAddressDataDTO.DeliveryPoints != null && postalAddressDataDTO.DeliveryPoints.Count > 0)
                        {
                            DeliveryPointDTO deliveryPointDTO = await thirdPartyAddressLocationIntegrationService.GetDeliveryPointByPostalAddress(postalAddressDataDTO.ID);

                            // Check if the existing delivery point has an approx location.
                            if (deliveryPointDTO.OperationalStatus_GUID == realLocation)
                            {
                                deliveryPointDTO.LocationProvider_GUID = locationProviderInternalId;

                                // Update the location provider for the delivery point
                                await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointById(deliveryPointDTO);

                                NotificationDTO notificationDO = new NotificationDTO
                                {
                                    ID = Guid.NewGuid(),
                                    NotificationType_GUID = notificationTypeId_GUID,
                                    NotificationDueDate = DateTime.UtcNow.AddHours(ThirdPartyAddressLocationConstants.NOTIFICATIONDUE),
                                    NotificationSource = ThirdPartyAddressLocationConstants.USRNOTIFICATIONSOURCE,
                                    Notification_Heading = ThirdPartyAddressLocationConstants.USRDELETEACTION,
                                    Notification_Message = AddressFields(postalAddressDTO),
                                    PostcodeDistrict = (postCodeSectorDTO == null || postCodeSectorDTO.District == null)
                                            ? string.Empty
                                            : postCodeSectorDTO.District,
                                    PostcodeSector = (postCodeSectorDTO == null || postCodeSectorDTO.Sector == null)
                                            ? string.Empty
                                            : postCodeSectorDTO.Sector,
                                    NotificationActionLink = string.Format(ThirdPartyAddressLocationConstants.USRNOTIFICATIONLINK, fileUdprn)
                                };

                                // Insert the new notification.
                                await thirdPartyAddressLocationIntegrationService.AddNewNotification(notificationDO);
                            }
                            else
                            {
                                // TODO : Confirm by Akash ashokan
                            }
                        }
                    }
                    else
                    {
                        // Log error
                        loggingHelper.Log(string.Format(ThirdPartyAddressLocationConstants.PAFERRORLOGMESSAGE, ThirdPartyAddressLocationConstants.PAFErrorMessageForAddressTypeUSRNotFound, fileUdprn, ThirdPartyAddressLocationConstants., FileType.Usr, DateTime.UtcNow), TraceEventType.Error);
                    }
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
            }
        }
    }
}