using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.PostalAddress.WebAPI.Utils;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        private const string Comma = ", ";

        #region Property Declarations

        private IPostalAddressDataService addressDataService = default(IPostalAddressDataService);
        private IFileProcessingLogDataService fileProcessingLogDataService = default(IFileProcessingLogDataService);

        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IPostalAddressIntegrationService postalAddressIntegrationService = default(IPostalAddressIntegrationService);

        #endregion Property Declarations

        #region Constructor

        public PostalAddressBusinessService(
            IPostalAddressDataService addressDataService,
            IFileProcessingLogDataService fileProcessingLogDataService,
            ILoggingHelper loggingHelper,
            IConfigurationHelper configurationHelper,
            IHttpHandler httpHandler,
            IPostalAddressIntegrationService postalAddressIntegrationService)
        {
            this.addressDataService = addressDataService;
            this.fileProcessingLogDataService = fileProcessingLogDataService;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.httpHandler = httpHandler;
            this.postalAddressIntegrationService = postalAddressIntegrationService;
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(int? uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddress"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddress);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressDataDTO postalAddressDBDTO = await addressDataService.GetPostalAddress(uDPRN);

                var postalAddress = ConvertDataDTOToDTO(postalAddressDBDTO);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePostalAddressForNYB(List<PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);

            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePostalAddressForNYB"))
                {
                    string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SavePostalAddressForNYB);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);
                    List<string> categoryNamesSimpleLists = new List<string>
                    {
                        PostalAddressConstants.PostalAddressType,
                        PostalAddressConstants.PostalAddressStatus
                    };

                    Guid addressTypeId = GetReferenceData(categoryNamesSimpleLists, PostalAddressConstants.PostalAddressType, FileType.Nyb.ToString());
                    Guid addressStatusId = GetReferenceData(categoryNamesSimpleLists, PostalAddressConstants.PostalAddressStatus, PostCodeStatus.Live.GetDescription());

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<PostalAddressDTO, PostalAddressDataDTO>();
                    });
                    Mapper.Configuration.CreateMapper();
                    List<PostalAddressDataDTO> lstPostalAddressDBDTO = Mapper.Map<List<PostalAddressDTO>, List<PostalAddressDataDTO>>(lstPostalAddress);

                    if (lstPostalAddressDBDTO != null && lstPostalAddressDBDTO.Count > 0)
                    {
                        List<int> lstUDPRNS = lstPostalAddressDBDTO.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
                        if (!lstUDPRNS.All(a => a == 0))
                        {
                            foreach (var postalAddress in lstPostalAddressDBDTO)
                            {
                                postalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(postalAddress.ID, addressStatusId));
                                postalAddress.AddressType_GUID = addressTypeId;
                                await addressDataService.SaveAddress(postalAddress, strFileName);
                            }

                            isPostalAddressInserted = await addressDataService.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                        }
                    }

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                this.loggingHelper.Log(postalAddressList, TraceEventType.Information);
                throw;
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Business rules for PAF details
        /// </summary>
        /// <param name="lstPostalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePAFDetails"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SavePAFDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isPostalAddressProcessed = false;
                string postalAddressList = new JavaScriptSerializer() { MaxJsonLength = 50000000 }.Serialize(lstPostalAddress);
                try
                {
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressType).Result;

                    Guid addressTypeUSR = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Usr.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypePAF = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Paf.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypeNYB = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();

                    foreach (var item in lstPostalAddress)
                    {
                        loggingHelper.Log("record no " + lstPostalAddress.IndexOf(item) + " , Udprn :" + item.UDPRN, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                        await SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                    }

                    isPostalAddressProcessed = true;

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                    this.loggingHelper.Log(postalAddressList, TraceEventType.Information);
                    throw;
                }

                return isPostalAddressProcessed;
            }
        }

        /// <summary>
        /// Method implementation to save delivery point and Task for notification for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">pass PostalAddreesDTO</param>
        public async Task SaveDeliveryPointProcess(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SaveDeliveryPointProcess"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SaveDeliveryPointProcess);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                // Call postalAddressIntegrationService to get reference data
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                       ReferenceDataCategoryNames.TASKNOTIFICATION,
                       ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER,
                       ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.TASKNOTIFICATION, PostalAddressConstants.TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER, PostalAddressConstants.EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointUseIndicator, PostalAddressConstants.DeliveryPointUseIndicatorPAF, true);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLive, true);
                Guid operationalStatusGUIDLivePending = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLivePending, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, PostalAddressConstants.NetworkNodeTypeRMGServiceNode, true);

                // Search Address Location for Postal Address If found, Add delivery point as per Address
                // Location details Else, Add delivery point w/o Address location details and also add
                // task in notification
                var objAddressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? default(int)).Result;

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);

                if (objAddressLocation == null)
                {
                    // deriving an approximate Location for the Delivery Point
                    DbGeometry approxLocation = await postalAddressIntegrationService.GetApproxLocation(objPostalAddress.Postcode);

                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
                        LocationXY = approxLocation,
                        OperationalStatus_GUID = operationalStatusGUIDLivePending,
                        NetworkNodeType_GUID = networkNodeTypeRMGServiceNode
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);

                    // Create task
                    var objTask = new NotificationDTO
                    {
                        ID = Guid.NewGuid(),
                        NotificationType_GUID = tasktypeId,
                        NotificationPriority_GUID = null,
                        NotificationSource = PostalAddressConstants.TASKSOURCE,
                        Notification_Heading = PostalAddressConstants.TASKPAFACTION,
                        Notification_Message = AddressFields(objPostalAddress),
                        PostcodeDistrict = postCodeDistrict,
                        NotificationDueDate = DateTime.UtcNow.AddHours(PostalAddressConstants.NOTIFICATIONDUE),
                        NotificationActionLink = string.Format(PostalAddressConstants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
                    };

                    // Call Notification service
                    await postalAddressIntegrationService.AddNewNotification(objTask);
                }
                else
                {
                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        LocationXY = objAddressLocation.LocationXY,
                        Latitude = objAddressLocation.Lattitude,
                        Longitude = objAddressLocation.Longitude,
                        LocationProvider_GUID = locationProviderId,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,

                        OperationalStatus_GUID = operationalStatusGUIDLive,
                        NetworkNodeType_GUID = networkNodeTypeRMGServiceNode
                    };
                    loggingHelper.Log(PostalAddressConstants.OperationalStatusGUIDLive.ToString(), TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SaveDeliveryPointProcessPriority, LoggerTraceConstants.SaveDeliveryPointProcessBusinessMethodExitEventId, LoggerTraceConstants.Title);
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="postalAddressId">PostalAddress Unique Identifier</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid postalAddressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddressDetails"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddressDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var postalAddressDBDTO = addressDataService.GetPostalAddressDetails(postalAddressId);

                var postalAddress = ConvertDataDTOToDTO(postalAddressDBDTO);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        public async Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateNybRecords"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressType).Result;
                Guid addressTypeNYB = GetReferenceData(PostalAddressConstants.PostalAddressType, FileType.Nyb.ToString());

                string postCode = string.Empty;
                var objDataPostalAddress = ConvertDTOToDataDTO(objPostalAddress);
                postCode = addressDataService.CheckForDuplicateNybRecords(objDataPostalAddress, addressTypeNYB);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postCode;
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateAddressWithDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isDuplicate = await addressDataService.CheckForDuplicateAddressWithDeliveryPoints(ConvertDTOToDataDTO(objPostalAddress));
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return isDuplicate;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public CreateDeliveryPointModelDTO CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CreateAddressForDeliveryPoint"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CreateAddressForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isAddressLocationAvailable = false;
                double? addLocationXCoOrdinate = 0;
                double? addLocationYCoOrdinate = 0;
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus, ReferenceDataCategoryNames.PostalAddressAliasType };

                Guid usrAddressTypeId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressType, FileType.Usr.ToString().ToUpper(), true);
                Guid liveAddressStatusId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressStatus, PostalAddressConstants.LiveAddressStatus, true);
                Guid deliveryPointAliasId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressAliasType, PostalAddressConstants.DeliveryPointAlias, false);

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null)
                {
                    addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
                    addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID = usrAddressTypeId;
                    // addDeliveryPointDTO.PostalAddressDTO.PostalAddressStatus.Add(GetPostalAddressStatus(addDeliveryPointDTO.PostalAddressDTO.ID, liveAddressStatusId));

                    addDeliveryPointDTO.PostalAddressDTO.AddressStatus_GUID = liveAddressStatusId;
                    addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias = addDeliveryPointDTO.PostalAddressAliasDTOs;
                    addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias.ForEach(p => p.AliasTypeGUID = deliveryPointAliasId);
                }

                var postalAddressId = addressDataService.CreateAddressForDeliveryPoint(ConvertToDataDTO(addDeliveryPointDTO.PostalAddressDTO));

                // check if third partylocation exists
                var addressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(addDeliveryPointDTO.PostalAddressDTO.UDPRN ?? default(int)).Result;

                if (addressLocation == null)
                {
                    isAddressLocationAvailable = false;
                }
                else
                {
                    isAddressLocationAvailable = true;
                    addLocationXCoOrdinate = Convert.ToDouble(addressLocation.Lattitude);
                    addLocationYCoOrdinate = Convert.ToDouble(addressLocation.Longitude);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);

                return new CreateDeliveryPointModelDTO { ID = postalAddressId, IsAddressLocationAvailable = isAddressLocationAvailable, XCoordinate = addLocationXCoOrdinate, YCoordinate = addLocationYCoOrdinate };
            }
        }

        /// <summary>
        /// Get Postal Address for the List of Guids
        /// </summary>
        /// <param name="addressGuids">List of Address Guids</param>
        /// <returns></returns>
        public async Task<List<PostalAddressDTO>> GetPostalAddresses(List<Guid> addressGuids)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddresses"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddresses);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var addressDetails = await addressDataService.GetPostalAddresses(addressGuids);

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDataDTO, PostalAddressDTO>();
                });
                Mapper.Configuration.CreateMapper();

                var listAddressDetails = Mapper.Map<List<PostalAddressDataDTO>, List<PostalAddressDTO>>(addressDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return listAddressDetails;
            }
        }

        /// <summary>
        /// Get PAF Addresses on UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN number</param>
        /// <returns>Postal Address DTO</returns>
        public async Task<PostalAddressDTO> GetPAFAddress(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddresses"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPAFAddress);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                Guid addressTypePAF = GetReferenceData(PostalAddressConstants.PostalAddressType, FileType.Paf.ToString());

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return await addressDataService.GetPAFAddress(udprn, addressTypePAF);
            }
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Business rule implementation for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO to process</param>
        /// <param name="addressTypeUSR">addressType Guid for USR</param>
        /// <param name="addressTypeNYB">addressType Guid for NYB</param>
        /// <param name="addressTypePAF">addressType Guid for PAF</param>
        /// <param name="strFileName">FileName on PAF events to track against DB</param>
        private async Task SavePAFRecords(PostalAddressDTO objPostalAddressBatch, Guid addressTypeUSR, Guid addressTypeNYB, Guid addressTypePAF, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePAFRecords"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SavePAFRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                FileProcessingLogDTO objFileProcessingLog = null;
                Guid deliveryPointUseIndicatorPAF = Guid.Empty;
                Guid postCodeGuid = Guid.Empty;

                // Construct New PostalAddressDTO
                PostalAddressDataDTO objPostalAddress = new PostalAddressDataDTO
                {
                    Postcode = objPostalAddressBatch.Postcode,
                    PostTown = objPostalAddressBatch.PostTown,
                    DependentLocality = objPostalAddressBatch.DependentLocality,
                    DoubleDependentLocality = objPostalAddressBatch.DoubleDependentLocality,
                    Thoroughfare = objPostalAddressBatch.Thoroughfare,
                    DependentThoroughfare = objPostalAddressBatch.DependentThoroughfare,
                    BuildingNumber = objPostalAddressBatch.BuildingNumber,
                    BuildingName = objPostalAddressBatch.BuildingName,
                    SubBuildingName = objPostalAddressBatch.SubBuildingName,
                    POBoxNumber = objPostalAddressBatch.POBoxNumber,
                    DepartmentName = objPostalAddressBatch.DepartmentName,
                    OrganisationName = objPostalAddressBatch.OrganisationName,
                    UDPRN = objPostalAddressBatch.UDPRN,
                    PostcodeType = objPostalAddressBatch.PostcodeType,
                    SmallUserOrganisationIndicator = objPostalAddressBatch.SmallUserOrganisationIndicator,
                    DeliveryPointSuffix = objPostalAddressBatch.DeliveryPointSuffix,
                    RowCreateDateTime = DateTime.UtcNow
                };

                // Address type will be PAF only in case of Inserting and updating records
                objPostalAddress.AddressType_GUID = addressTypePAF;
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressStatus).Result;
                var addressStatus_GUID = GetReferenceData(PostalAddressConstants.PostalAddressStatus, PostCodeStatus.Live.ToString());

                // match if PostalAddress exists on UDPRN match
                PostalAddressDataDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                // match if PostalAddress exists on Address match
                var objPostalAddressMatchedAddress = await addressDataService.GetPostalAddress(objPostalAddress);

                // PAF process Logic
                if (objPostalAddressMatchedUDPRN != null)
                {
                    if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                    {
                        objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                        objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                        if (await addressDataService.UpdateAddress(objPostalAddress, strFileName))
                        {
                            // calling delivery point web api
                            var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);
                            if (objDeliveryPoint == null)
                            {
                                await SaveDeliveryPointProcess(objPostalAddress);
                            }
                        }
                    }
                    else
                    {
                        objFileProcessingLog = new FileProcessingLogDTO
                        {
                            FileID = Guid.NewGuid(),
                            UDPRN = objPostalAddress.UDPRN ?? default(int),
                            AmendmentType = objPostalAddressBatch.AmendmentType,
                            FileName = strFileName,
                            FileProcessing_TimeStamp = DateTime.UtcNow,
                            FileType = FileType.Paf.ToString(),
                            ErrorMessage = PostalAddressConstants.PAFErrorMessageForAddressTypeNYBNotFound,
                            SuccessFlag = false
                        };

                        fileProcessingLogDataService.LogFileException(objFileProcessingLog);
                    }
                }
                else if (objPostalAddressMatchedAddress != null)
                {
                    if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                    {
                        objPostalAddress.ID = objPostalAddressMatchedAddress.ID;
                        objPostalAddress.PostalAddressStatus = objPostalAddressMatchedAddress.PostalAddressStatus;

                        // Update Postal address
                        if (await addressDataService.UpdateAddress(objPostalAddress, strFileName))
                        {
                            // Update delivery point for USR records
                            await UpdateDeliveryPointProcess(objPostalAddress);
                        }

                        /*else
                        {
                            objFileProcessingLog = new FileProcessingLogDTO
                            {
                                FileID = Guid.NewGuid(),
                                UDPRN = objPostalAddress.UDPRN ?? default(int),
                                AmendmentType = objPostalAddressBatch.AmendmentType,
                                FileName = strFileName,
                                FileProcessing_TimeStamp = DateTime.UtcNow,
                                FileType = FileType.Paf.ToString(),
                                ErrorMessage = PostalAddressConstants.PAFErrorMessageForUnmatchedDeliveryPointForUSRType,
                                SuccessFlag = false
                            };

                            fileProcessingLogDataService.LogFileException(objFileProcessingLog);
                        }*/
                    }
                    else
                    {
                        objFileProcessingLog = new FileProcessingLogDTO
                        {
                            FileID = Guid.NewGuid(),
                            UDPRN = objPostalAddress.UDPRN ?? default(int),
                            AmendmentType = objPostalAddressBatch.AmendmentType,
                            FileName = strFileName,
                            FileProcessing_TimeStamp = DateTime.UtcNow,
                            FileType = FileType.Paf.ToString(),
                            ErrorMessage = PostalAddressConstants.PAFErrorMessageForAddressTypeUSRNotFound,
                            SuccessFlag = false
                        };

                        fileProcessingLogDataService.LogFileException(objFileProcessingLog);
                    }
                }
                else
                {
                    objPostalAddress.ID = Guid.NewGuid();
                    objPostalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(objPostalAddress.ID, addressStatus_GUID));
                    await addressDataService.InsertAddress(objPostalAddress, strFileName);
                    await SaveDeliveryPointProcess(objPostalAddress);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Updating Delivery Point to the database in case of USR records
        /// </summary>
        /// <param name="objPostalAddress">Postal Address DTO</param>
        /// <returns>nothing</returns>
        private async Task UpdateDeliveryPointProcess(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.UpdateDeliveryPointProcess"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(UpdateDeliveryPointProcess);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                       ReferenceDataCategoryNames.TASKNOTIFICATION,
                       ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER,
                       ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);
                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.TASKNOTIFICATION, PostalAddressConstants.TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER, PostalAddressConstants.EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointUseIndicator, PostalAddressConstants.DeliveryPointUseIndicatorPAF, true);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ReferenceDataCategoryNames.NetworkNodeType, true);
                Guid operationalStatusGUIDLivePending = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLivePending, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, PostalAddressConstants.NetworkNodeTypeRMGServiceNode, true);

                var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);

                if (objDeliveryPoint != null && objDeliveryPoint.OperationalStatus_GUID == operationalStatusGUIDLivePending)
                {
                    AddressLocationDTO addressLocationDTO = await postalAddressIntegrationService.GetAddressLocationByUDPRN((int)objPostalAddress.UDPRN);

                    if (addressLocationDTO != null)
                    {
                        // DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO
                        // {
                        // ID = Guid.NewGuid(),
                        objDeliveryPoint.Address_GUID = objPostalAddress.ID;
                        objDeliveryPoint.LocationXY = addressLocationDTO.LocationXY;
                        objDeliveryPoint.LocationProvider_GUID = locationProviderId;
                        objDeliveryPoint.DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator;
                        objDeliveryPoint.OperationalStatus_GUID = operationalStatusGUIDLive;

                        objDeliveryPoint.NetworkNodeType_GUID = networkNodeTypeRMGServiceNode;

                        // };
                        if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION))
                        {
                            await postalAddressIntegrationService.UpdateDeliveryPoint(objDeliveryPoint);

                            // update the notification if it exists.
                            await postalAddressIntegrationService.UpdateNotificationByUDPRN((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION, PostalAddressConstants.NOTIFICATIONCLOSED);
                        }
                    }
                    else
                    {
                        // var objTask = new NotificationDTO
                        // {
                        //    ID = Guid.NewGuid(),
                        //    NotificationType_GUID = tasktypeId,
                        //    NotificationPriority_GUID = null,
                        //    NotificationSource = Constants.TASKSOURCE,
                        //    Notification_Heading = Constants.TASKPAFACTION,
                        //    Notification_Message = AddressFields(objPostalAddress),
                        //    PostcodeDistrict = postCodeDistrict,
                        //    NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE),
                        //    NotificationActionLink = string.Format(Constants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
                        // };
                        if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION))
                        {
                            string message = AddressFields(objPostalAddress);
                            await postalAddressIntegrationService.UpdateNotificationMessageByUDPRN((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION, message);
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.AddressFields"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(AddressFields);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                string pafMessage = PostalAddressConstants.PAFTaskBodyPreText +
                        objPostalAddress.OrganisationName + Comma +
                        objPostalAddress.DepartmentName + Comma +
                        objPostalAddress.BuildingName + Comma +
                        objPostalAddress.BuildingNumber + Comma +
                        objPostalAddress.SubBuildingName + Comma +
                        objPostalAddress.Thoroughfare + Comma +
                        objPostalAddress.DependentThoroughfare + Comma +
                        objPostalAddress.DependentLocality + Comma +
                        objPostalAddress.DoubleDependentLocality + Comma +
                        objPostalAddress.PostTown + Comma +
                        objPostalAddress.Postcode;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return pafMessage;
            }
        }

        /// <summary>
        /// Construct Postal Address Status for resp Postal Address
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns postal address DTO</returns>
        private PostalAddressStatusDataDTO GetPostalAddressStatus(Guid postalAddressGUID, Guid operationalStatusGUID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddressStatus"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddressStatus);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressStatusDataDTO postalAddressStatusDataDTO = new PostalAddressStatusDataDTO
                {
                    ID = Guid.NewGuid(),
                    PostalAddressGUID = postalAddressGUID,
                    OperationalStatusGUID = operationalStatusGUID,
                    StartDateTime = DateTime.UtcNow,
                    RowCreateDateTime = DateTime.UtcNow
                };

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddressStatusDataDTO;
            }
        }

        /// <summary>
        /// Get the Reference data for the categorynames based on the referencedata value
        /// </summary>
        /// <param name="categoryNamesSimpleLists">list of category names</param>
        /// <param name="categoryName">category name</param>
        /// <param name="referenceDataValue">reference data value</param>
        /// <param name="isWithSpace"> whether </param>
        /// <returns></returns>
        private Guid GetReferenceData(List<string> categoryNamesSimpleLists, string categoryName, string referenceDataValue, bool isWithSpace = false)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
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

        private Guid GetReferenceData(string categoryName, string referenceDataValue)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryName).Result;
                Guid referenceDataGuid = referenceDataCategoryList.ReferenceDatas
                    .Where(a => a.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ID).FirstOrDefault();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return referenceDataGuid;
            }
        }

        /// <summary>
        /// Method to convert DTO to DataDTO of PostalAddress
        /// </summary>
        /// <param name="objPostalAddress"></param>
        /// <returns></returns>
        private PostalAddressDataDTO ConvertDTOToDataDTO(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ConvertDTOToDataDTO);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressDataDTO postalAddressDBDTO = new PostalAddressDataDTO();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDTO, PostalAddressDataDTO>();
                });
                Mapper.Configuration.CreateMapper();

                postalAddressDBDTO = Mapper.Map<PostalAddressDTO, PostalAddressDataDTO>(objPostalAddress);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddressDBDTO;
            }
        }

        /// <summary>
        /// Method to convert DataDTO to DTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDBDTO">PostalAddressDataDTO object</param>
        /// <returns></returns>
        private PostalAddressDTO ConvertDataDTOToDTO(PostalAddressDataDTO postalAddressDBDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ConvertDataDTOToDTO);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressDTO postalAddressDTO = new PostalAddressDTO();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDataDTO, PostalAddressDTO>();
                });
                Mapper.Configuration.CreateMapper();

                postalAddressDTO = Mapper.Map<PostalAddressDataDTO, PostalAddressDTO>(postalAddressDBDTO);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddressDTO;
            }
        }

        /// <summary>
        /// Method to convert DTO to DataDTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDTO">PostalAddress DTO Object</param>
        /// <returns>returns converted PostalAddressDataDTO object</returns>
        private PostalAddressDataDTO ConvertToDataDTO(PostalAddressDTO postalAddressDTO)
        {
            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO();
            if (postalAddressDTO != null)
            {
                postalAddressDataDTO.ID = postalAddressDTO.ID;

                // postalAddressDataDTO.AddressStatus_GUID = postalAddressDTO.AddressStatus_GUID;
                postalAddressDataDTO.AddressType_GUID = postalAddressDTO.AddressType_GUID;
                postalAddressDataDTO.AMUApproved = postalAddressDTO.AMUApproved;
                postalAddressDataDTO.BuildingName = postalAddressDTO.BuildingName;
                postalAddressDataDTO.BuildingNumber = postalAddressDTO.BuildingNumber;
                postalAddressDataDTO.DeliveryPointSuffix = postalAddressDTO.DeliveryPointSuffix;
                postalAddressDataDTO.DepartmentName = postalAddressDTO.DepartmentName;
                postalAddressDataDTO.DependentLocality = postalAddressDTO.DependentLocality;
                postalAddressDataDTO.DependentThoroughfare = postalAddressDTO.DependentThoroughfare;
                postalAddressDataDTO.DoubleDependentLocality = postalAddressDTO.DoubleDependentLocality;
                postalAddressDataDTO.OrganisationName = postalAddressDTO.OrganisationName;
                postalAddressDataDTO.POBoxNumber = postalAddressDTO.POBoxNumber;
                postalAddressDataDTO.Postcode = postalAddressDTO.Postcode;

                // postalAddressDataDTO.pos = postalAddressDTO.PostCodeGUID;
                postalAddressDataDTO.PostcodeType = postalAddressDTO.PostcodeType;
                postalAddressDataDTO.PostTown = postalAddressDTO.PostTown;
                postalAddressDataDTO.SmallUserOrganisationIndicator = postalAddressDTO.SmallUserOrganisationIndicator;
                postalAddressDataDTO.SubBuildingName = postalAddressDTO.SubBuildingName;
                postalAddressDataDTO.Thoroughfare = postalAddressDTO.Thoroughfare;
                postalAddressDataDTO.UDPRN = postalAddressDTO.UDPRN;

                PostalAddressStatusDataDTO postalAddressStatusDataDTO = new PostalAddressStatusDataDTO();
                postalAddressStatusDataDTO.PostalAddressGUID = postalAddressDTO.ID;
                postalAddressStatusDataDTO.OperationalStatusGUID = postalAddressDTO.AddressStatus_GUID.HasValue ? postalAddressDTO.AddressStatus_GUID.Value : Guid.Empty;
                postalAddressDataDTO.PostalAddressStatus.Add(postalAddressStatusDataDTO);

                if (postalAddressDTO.PostalAddressAlias != null)
                {
                    List<PostalAddressAliasDataDTO> postalAddressAliases = new List<PostalAddressAliasDataDTO>();
                    foreach (var postalAddressAliasDTO in postalAddressDTO.PostalAddressAlias)
                    {
                        PostalAddressAliasDataDTO postalAddressAlias = new PostalAddressAliasDataDTO
                        {
                            ID=Guid.NewGuid(),
                            PostalAddressID = postalAddressDTO.ID,
                            AliasTypeGUID = postalAddressAliasDTO.AliasTypeGUID,
                            AliasName = postalAddressAliasDTO.AliasName,
                            PreferenceOrderIndex = postalAddressAliasDTO.PreferenceOrderIndex
                        };
                        postalAddressAliases.Add(postalAddressAlias);
                    }

                    postalAddressDataDTO.PostalAddressAlias = postalAddressAliases;
                }

            }

            return postalAddressDataDTO;
        }

        /// <summary>
        /// Method to convert List od DTO to List of DataDTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDTOList">List of PostalAddress DTO Object</param>
        /// <returns>returns converted List of PostalAddressDataDTO object</returns>
        private List<PostalAddressDataDTO> ConvertToDataDTO(List<PostalAddressDTO> postalAddressDTOList)
        {
            List<PostalAddressDataDTO> postalAddressDataDTO = new List<PostalAddressDataDTO>();

            foreach (var postalAddressDTO in postalAddressDTOList)
            {
                postalAddressDataDTO.Add(ConvertToDataDTO(postalAddressDTO));
            }

            return postalAddressDataDTO;
        }

        /// <summary>
        /// Method to convert DataDTO to DTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDataDTO">postalAddressDataDTO Object</param>
        /// <returns>returns converted PostalAddressDTO object</returns>
        private PostalAddressDTO ConvertToDTO(PostalAddressDataDTO postalAddressDataDTO)
        {
            PostalAddressDTO postalAddressDTO = new PostalAddressDTO();

            if (postalAddressDataDTO != null)
            {
                postalAddressDTO.ID = postalAddressDataDTO.ID;

                // postalAddressDataDTO.AddressStatus_GUID = postalAddressDTO.AddressStatus_GUID;
                postalAddressDTO.AddressType_GUID = postalAddressDataDTO.AddressType_GUID;
                postalAddressDTO.AMUApproved = postalAddressDataDTO.AMUApproved;
                postalAddressDTO.BuildingName = postalAddressDataDTO.BuildingName;
                postalAddressDTO.BuildingNumber = postalAddressDataDTO.BuildingNumber;
                postalAddressDTO.DeliveryPointSuffix = postalAddressDataDTO.DeliveryPointSuffix;
                postalAddressDTO.DepartmentName = postalAddressDataDTO.DepartmentName;
                postalAddressDTO.DependentLocality = postalAddressDataDTO.DependentLocality;
                postalAddressDTO.DependentThoroughfare = postalAddressDataDTO.DependentThoroughfare;
                postalAddressDTO.DoubleDependentLocality = postalAddressDataDTO.DoubleDependentLocality;
                postalAddressDTO.OrganisationName = postalAddressDataDTO.OrganisationName;
                postalAddressDTO.POBoxNumber = postalAddressDataDTO.POBoxNumber;
                postalAddressDTO.Postcode = postalAddressDataDTO.Postcode;

                // postalAddressDataDTO.pos = postalAddressDTO.PostCodeGUID;
                postalAddressDTO.PostcodeType = postalAddressDataDTO.PostcodeType;
                postalAddressDTO.PostTown = postalAddressDataDTO.PostTown;
                postalAddressDTO.SmallUserOrganisationIndicator = postalAddressDataDTO.SmallUserOrganisationIndicator;
                postalAddressDTO.SubBuildingName = postalAddressDataDTO.SubBuildingName;
                postalAddressDTO.Thoroughfare = postalAddressDataDTO.Thoroughfare;
                postalAddressDTO.UDPRN = postalAddressDataDTO.UDPRN;

                if (postalAddressDataDTO.PostalAddressStatus != null)
                {
                    postalAddressDTO.AddressStatus_GUID = postalAddressDataDTO.PostalAddressStatus.First().OperationalStatusGUID;
                }
            }

            return postalAddressDTO;
        }

        /// <summary>
        /// Method to convert List of DataDTO to List of DTO for PostalAddress
        /// </summary>
        /// <param name="postalAddressDataDTOList">List of postalAddressDataDTO Object</param>
        /// <returns>returns converted List of PostalAddressDTO object</returns>
        private List<PostalAddressDTO> ConvertToDTO(List<PostalAddressDataDTO> postalAddressDataDTOList)
        {
            List<PostalAddressDTO> postalAddressDTO = new List<PostalAddressDTO>();

            foreach (var postalAddressDataDTO in postalAddressDataDTOList)
            {
                postalAddressDTO.Add(ConvertToDTO(postalAddressDataDTO));
            }

            return postalAddressDTO;
        }

        #endregion private methods
    }
}