using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO.FileProcessing;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Utils;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService;
using System.Linq;

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                AddressLocationDataDTO addressLocationDataDto = await this.addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
                AddressLocationDTO addressLocationDto = GenericMapper.Map<AddressLocationDataDTO, AddressLocationDTO>(addressLocationDataDto);
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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                var addressLocationDataDto = await addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
                AddressLocationDTO addressLocationDto = GenericMapper.Map<AddressLocationDataDTO, AddressLocationDTO>(addressLocationDataDto);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return addressLocationDto;
            }
        }

        #region Save USR Details to Database

        // To be implemented in parallel
        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos">List of Address Locations</param>
        /// <returns>Task</returns>
        /*public async Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            int fileUdprn;
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveUSRDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

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


                    PostalAddressDTO postalAddressDTONew = await thirdPartyAddressLocationIntegrationService.GetPAFAddress((int)fileUdprn);

                    // Check if the delivery point exists
                    if (postalAddressDTONew != null)
                    {
                        DeliveryPointDTO deliveryPointDTO = await thirdPartyAddressLocationIntegrationService.GetDeliveryPointByPostalAddress((Guid)postalAddressDTONew.ID);
                        //await thirdPartyAddressLocationIntegrationService.GetDeliveryPointByUDPRNForThirdParty((int)fileUdprn);

                        List<string> categoryNamesSimpleLists = new List<string>
                            {
                                ThirdPartyAddressLocationConstants.TASKNOTIFICATION,
                                ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER,
                                ThirdPartyAddressLocationConstants.DeliveryPointUseIndicator,
                                ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                                ReferenceDataCategoryNames.NetworkNodeType
                            };

                        var referenceDataCategoryList = thirdPartyAddressLocationIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                        Guid tasktypeId = referenceDataCategoryList
                                        .Where(list => list.CategoryName.Equals(ThirdPartyAddressLocationConstants.TASKNOTIFICATION, StringComparison.OrdinalIgnoreCase))
                                        .SelectMany(list => list.ReferenceDatas)
                                        .Where(item => item.ReferenceDataValue.Equals(ThirdPartyAddressLocationConstants.TASKACTION, StringComparison.OrdinalIgnoreCase))
                                        .Select(s => s.ID).SingleOrDefault();
                        Guid locationProviderId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(ThirdPartyAddressLocationConstants.NETWORKLINKDATAPROVIDER, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(ThirdPartyAddressLocationConstants.EXTERNAL, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                        Guid deliveryPointUseIndicator = referenceDataCategoryList
                                        .Where(list => list.CategoryName.Equals(ThirdPartyAddressLocationConstants.DeliveryPointUseIndicator, StringComparison.OrdinalIgnoreCase))
                                        .SelectMany(list => list.ReferenceDatas)
                                        .Where(item => item.ReferenceDataValue.Equals(ThirdPartyAddressLocationConstants.DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                        .Select(s => s.ID).SingleOrDefault();
                        Guid OperationalStatusGUIDLive = referenceDataCategoryList
                                        .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                        .SelectMany(list => list.ReferenceDatas)
                                        .Where(item => item.ReferenceDataValue.Equals(ThirdPartyAddressLocationConstants.OperationalStatusGUIDLive, StringComparison.OrdinalIgnoreCase))
                                        .Select(s => s.ID).SingleOrDefault();
                        Guid NetworkNodeTypeRMGServiceNode = referenceDataCategoryList
                                        .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkNodeType)
                                        .SelectMany(list => list.ReferenceDatas)
                                        .Where(item => item.ReferenceDataValue.Equals(ThirdPartyAddressLocationConstants.NetworkNodeTypeRMGServiceNode, StringComparison.OrdinalIgnoreCase))
                                        .Select(s => s.ID).SingleOrDefault();

                        // Check if the existing delivery point has a location.
                        if (deliveryPointDTO == null)
                        {
                            // Get the Location provider Guid from the reference data table.
                            //Guid locationProviderId =
                            //    await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.EXTERNAL);
                            

                            DeliveryPointDTO newDeliveryPoint = new DeliveryPointDTO
                            {
                                ID = Guid.NewGuid(),
                                Latitude = addressLocationUSRPOSTDTO.Latitude,
                                Longitude = addressLocationUSRPOSTDTO.Longitude,
                                LocationXY = spatialLocationXY,
                                Address_GUID = postalAddressDTONew.ID,
                                LocationProvider_GUID = locationProviderId,
                                UDPRN = fileUdprn,
                                DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
                                OperationalStatus_GUID = OperationalStatusGUIDLive,
                                NetworkNodeType_GUID = NetworkNodeTypeRMGServiceNode
                            };

                            // Update the location details for the delivery point
                            await thirdPartyAddressLocationIntegrationService.InsertDeliveryPoint(newDeliveryPoint);

                            // Check if a notification exists for the UDPRN.
                            if (await thirdPartyAddressLocationIntegrationService.CheckIfNotificationExists(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION))
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
                            if (straightLineDistance < ThirdPartyAddressLocationConstants.TOLERANCEDISTANCEINMETERS)
                            {
                                //Guid locationProviderId = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(
                                //    Constants.NETWORKLINKDATAPROVIDER,
                                //    Constants.EXTERNAL);

                                DeliveryPointDTO newDeliveryPoint = new DeliveryPointDTO
                                {
                                    ID = Guid.NewGuid(),
                                    Latitude = addressLocationUSRPOSTDTO.Latitude,
                                    Longitude = addressLocationUSRPOSTDTO.Longitude,
                                    LocationXY = spatialLocationXY,
                                    Address_GUID = postalAddressDTONew.ID,
                                    LocationProvider_GUID = locationProviderId,
                                    UDPRN = fileUdprn,
                                    DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
                                    OperationalStatus_GUID = OperationalStatusGUIDLive,
                                    NetworkNodeType_GUID = NetworkNodeTypeRMGServiceNode
                                };

                                if (await thirdPartyAddressLocationIntegrationService.DeleteDeliveryPoint(deliveryPointDTO.ID))
                                {
                                    await thirdPartyAddressLocationIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                                }


                                // Update the delivery point location directly in case it is within
                                // the tolerance limits.
                                //await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointLocationOnUDPRN(newDeliveryPoint);

                                // Check if the notification exists for the given UDPRN.
                                if (await thirdPartyAddressLocationIntegrationService.CheckIfNotificationExists(fileUdprn, ThirdPartyAddressLocationConstants.TASKPAFACTION))
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

                                    // Get the Notification Type from the Reference data table
                                    Guid notificationTypeId_GUID = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(
                                        ThirdPartyAddressLocationConstants.USRCATEGORY,
                                        ThirdPartyAddressLocationConstants.USRREFERENCEDATANAME);

                                    PostalAddressDTO postalAddressDTO = await thirdPartyAddressLocationIntegrationService.GetPostalAddress(fileUdprn);

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
        }*/

        /// <summary>
        /// This method is used to fetch GeoJson data for Address Location.
        /// </summary>
        /// <param name="addressLocationDto">Address Location DTO</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetAddressLocationJsonData(AddressLocationDTO addressLocationDto)
        {
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

            return addressLocationGeoJson;
        }

        #endregion Save USR Details to Database

        #region Calculate Spatial Location

        /// <summary>
        /// Get the geometry equivalent of the X-Y co-ordinate of address location
        /// </summary>
        /// <param name="addressLocationUSRPOSTDTO">AddressLocationUSRPOSTDTO object</param>
        /// <returns>DbGeometry object</returns>
        private static DbGeometry GetSpatialLocation(AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO)
        {
            string sbLocationXY = string.Format(
                ThirdPartyAddressLocationConstants.USRGEOMETRYPOINT,
                Convert.ToString(addressLocationUSRPOSTDTO.XCoordinate),
                Convert.ToString(addressLocationUSRPOSTDTO.YCoordinate));

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), ThirdPartyAddressLocationConstants.BNGCOORDINATESYSTEM);
            return spatialLocationXY;
        }

        #endregion Calculate Spatial Location

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">USR create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDTO objPostalAddress)
        {
            return ThirdPartyAddressLocationConstants.USRNOTIFICATIONBODYPREFIX +
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
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="separator">separator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string separator)
        {
            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
        }

        /// <summary>
        /// Calculte distance between two points
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPoint DTO</param>
        /// <param name="newPoint">newPoint as DbGeometry</param>
        /// <returns>distance as double</returns>
        private double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint)
        {
            double? distance = 0;
            if (deliveryPointDTO != null)
            {
                distance = deliveryPointDTO.LocationXY.Distance(newPoint);
            }

            return distance;
        }
    }
}