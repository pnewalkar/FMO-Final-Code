using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;
using RM.DataManagement.PostalAddress.WebAPI.DTO.UIDropdowns;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using AutoMapper;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService //: IPostalAddressBusinessService
    {
        private const string TASKPAFACTION = "Position new DP";
        private const string DeliveryPointUseIndicatorPAF = "Organisation";
        private const string PAFErrorMessageForUnmatchedDeliveryPointForUSRType = "Delivery point not present for Postal address whose address type is <USR>";
        private const string PAFErrorMessageForAddressTypeNYBNotFound = "Address Type of the selected Postal Address record is not <NYB>";
        private const string PAFErrorMessageForAddressTypeUSRNotFound = "Address Type of the selected Postal Address record is not <USR>";
        private const string PAFTaskBodyPreText = "Please position the DP ";
        private const string PAFNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";
        private const int NOTIFICATIONDUE = 24;
        private const string NETWORKLINKDATAPROVIDER = "Data Provider";
        private const string EXTERNAL = "External";
        private const string TASKNOTIFICATION = "Notification Type";
        private const string TASKACTION = "Action required";
        private const string TASKSOURCE = "SYSTEM";
        private const string PostalAddressStatus = "Postal Address Status";
        private const string PostalAddressType = "Postal Address Type";
        private const string Comma = ", ";
        private const string DeliveryPointUseIndicator = "DeliveryPoint Use Indicator";
        private const string LiveAddressStatus = "Live";
        private const string OperationalStatusGUIDLive = "Live";
        private const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        private const string NOTIFICATIONCLOSED = "Closed";

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
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
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);
                    List<string> categoryNamesSimpleLists = new List<string>
                    {
                        PostalAddressType,
                        PostalAddressStatus
                    };

                    Guid addressTypeId = GetReferenceData(categoryNamesSimpleLists, PostalAddressType, FileType.Nyb.ToString());
                    Guid addressStatusId = GetReferenceData(categoryNamesSimpleLists, PostalAddressStatus, PostCodeStatus.Live.GetDescription());

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isPostalAddressProcessed = false;
                string postalAddressList = new JavaScriptSerializer() { MaxJsonLength = 50000000 }.Serialize(lstPostalAddress);
                try
                {
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressType).Result;

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
        public async Task SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SaveDeliveryPointProcess"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                // Call postalAddressIntegrationService to get reference data
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        TASKNOTIFICATION,
                        NETWORKLINKDATAPROVIDER,
                        DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, TASKNOTIFICATION, TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, NETWORKLINKDATAPROVIDER, EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, DeliveryPointUseIndicator, DeliveryPointUseIndicatorPAF);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, OperationalStatusGUIDLive, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, NetworkNodeTypeRMGServiceNode, true);

                // Search Address Location for Postal Address If found, Add delivery point as per Address
                // Location details Else, Add delivery point w/o Address location details and also add
                // task in notification
                var objAddressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? default(int)).Result;

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);

                if (objAddressLocation == null)
                {
                    /* Poc do not create dp if adddress location does not exist
                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        UDPRN = objPostalAddress.UDPRN,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,

                        OperationalStatus_GUID = OperationalStatusGUIDLive,
                        NetworkNodeType_GUID = NetworkNodeTypeRMGServiceNode
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);*/

                    // Create task
                    var objTask = new NotificationDTO
                    {
                        ID = Guid.NewGuid(),
                        NotificationType_GUID = tasktypeId,
                        NotificationPriority_GUID = null,
                        NotificationSource = TASKSOURCE,
                        Notification_Heading = TASKPAFACTION,
                        Notification_Message = AddressFields(objPostalAddress),
                        PostcodeDistrict = postCodeDistrict,
                        NotificationDueDate = DateTime.UtcNow.AddHours(NOTIFICATIONDUE),
                        NotificationActionLink = string.Format(PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
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
                        // UDPRN = objAddressLocation.UDPRN,
                        LocationXY = objAddressLocation.LocationXY,
                        Latitude = objAddressLocation.Lattitude,
                        Longitude = objAddressLocation.Longitude,
                        LocationProvider_GUID = locationProviderId,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,

                        OperationalStatus_GUID = operationalStatusGUIDLive,
                        NetworkNodeType_GUID = networkNodeTypeRMGServiceNode
                    };
                    loggingHelper.Log(OperationalStatusGUIDLive.ToString(), TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SaveDeliveryPointProcessPriority, LoggerTraceConstants.SaveDeliveryPointProcessBusinessMethodExitEventId, LoggerTraceConstants.Title);
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);

            }
        }

        // To be moved to Unit Manager
        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        //public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressSearchDetails"))
        //    {
        //        string methodName = MethodHelper.GetActualAsyncMethodName();
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

        //        try
        //        {
        //            List<string> listNames = new List<string> { FileType.Paf.ToString().ToUpper(), FileType.Nyb.ToString().ToUpper() };

        //            var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(ReferenceDataCategoryNames.PostalAddressType).Result;
        //            List<Guid> addresstypeIDs = referenceDataCategoryList.ReferenceDatas
        //            .Where(a => listNames.Contains(a.ReferenceDataValue))
        //            .Select(a => a.ID).ToList();
        //            List<Guid> postcodeGuids = await addressDataService.GetPostcodeGuids(searchText);
        //            List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postcodes = await postalAddressIntegrationService.GetPostcodes(unitGuid, postcodeGuids);
        //            return await addressDataService.GetPostalAddressSearchDetails(searchText, unitGuid, addresstypeIDs, postcodes);
        //        }
        //        catch (Exception ex)
        //        {
        //            this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
        //            throw;
        //        }
        //        finally
        //        {
        //            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
        //        }
        //    }
        //}

        // To be moved to unit manager
        /// <summary>
        /// Filter PostalAddress based on the post code
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        //public async Task<PostalAddressDBDTO> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
        //    {
        //        string methodName = MethodBase.GetCurrentMethod().Name;
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

        //        List<BindingEntity> nybDetails = new List<BindingEntity>();
        //        PostalAddressDBDTO postalAddressDto = null;
        //        var postCodeGuids = addressDataService.GetSelectedPostcode(selectedItem).Result;
        //        var selectedPostcode = await postalAddressIntegrationService.GetPostcodes(unitGuid, postCodeGuids);
        //        var postalAddressDetails = await addressDataService.GetPostalAddressDetails(selectedItem, unitGuid, selectedPostcode);
        //        Guid nybAddressTypeId = postalAddressIntegrationService.GetReferenceDataGuId(PostalAddressType, FileType.Nyb.ToString()).Result;
        //        if (postalAddressDetails != null && postalAddressDetails.Count > 0)
        //        {
        //            postalAddressDto = postalAddressDetails[0];
        //            foreach (var postalAddress in postalAddressDetails)
        //            {
        //                if (postalAddress.AddressType_GUID == nybAddressTypeId)
        //                {
        //                    string address = string.Join(",", Convert.ToString(postalAddress.BuildingNumber) ?? string.Empty, postalAddress.BuildingName, postalAddress.SubBuildingName);
        //                    string formattedAddress = Regex.Replace(address, ",+", ",").Trim(',');
        //                    nybDetails.Add(new BindingEntity { Value = postalAddress.ID, DisplayText = formattedAddress });
        //                }
        //            }

        //            nybDetails.OrderBy(n => n.DisplayText);
        //            nybDetails.Add(new BindingEntity { Value = Guid.Empty, DisplayText = "Not Shown" });
        //            postalAddressDto.NybAddressDetails = nybDetails;
        //        }

        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
        //        return postalAddressDto;
        //    }
        //}

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddressDetails"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                var postalAddressDBDTO = addressDataService.GetPostalAddressDetails(id);

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
        public string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateNybRecords"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressType).Result;
                Guid addressTypeNYB = GetReferenceData(PostalAddressType, FileType.Nyb.ToString());

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
        public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                bool isDuplicate = addressDataService.CheckForDuplicateAddressWithDeliveryPoints(ConvertDTOToDataDTO(objPostalAddress));
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return isDuplicate;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CreateAddressAndDeliveryPoint"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus };

                Guid usrAddressTypeId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressType, FileType.Usr.ToString().ToUpper(), true);
                Guid liveAddressStatusId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressStatus, LiveAddressStatus, true);

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null)
                {
                    addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
                    addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID = usrAddressTypeId;
                    addDeliveryPointDTO.PostalAddressDTO.PostalAddressStatus.Add(GetPostalAddressStatus(addDeliveryPointDTO.PostalAddressDTO.ID, liveAddressStatusId));
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return addressDataService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO, liveAddressStatusId);

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                Guid addressTypePAF = GetReferenceData(PostalAddressType, FileType.Paf.ToString());

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                    DeliveryPointSuffix = objPostalAddressBatch.DeliveryPointSuffix
                };


                // Address type will be PAF only in case of Inserting and updating records
                objPostalAddress.AddressType_GUID = addressTypePAF;
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressStatus).Result;
                var addressStatus_GUID = GetReferenceData(PostalAddressStatus, PostCodeStatus.Live.ToString());

                // match if PostalAddress exists on UDPRN match
                PostalAddressDataDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                // match if PostalAddress exists on Address match
                var objPostalAddressMatchedAddress = await addressDataService.GetPostalAddress(objPostalAddress);

                // PAF process Logic
                if (objPostalAddressMatchedUDPRN != null)
                {
                    if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                    {
                        deliveryPointUseIndicatorPAF = GetReferenceData(DeliveryPointUseIndicator, DeliveryPointUseIndicatorPAF);

                        objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                        objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                        if (await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF))
                        {
                            // calling delivery point web api
                            var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);
                            if (objDeliveryPoint == null)
                            {
                                await SaveDeliveryPointProcess(objPostalAddressBatch);
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
                            ErrorMessage = PAFErrorMessageForAddressTypeNYBNotFound,
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
                        var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);

                        if (objDeliveryPoint == null)
                        {
                            deliveryPointUseIndicatorPAF = GetReferenceData(DeliveryPointUseIndicator, DeliveryPointUseIndicatorPAF);

                            objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                            // Update address and delivery point for USR records
                            await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF);


                            //await postalAddressIntegrationService.UpdateDeliveryPoint(objPostalAddress.ID, deliveryPointUseIndicatorPAF);

                            //objFileProcessingLog = new FileProcessingLogDTO
                            //{
                            //    FileID = Guid.NewGuid(),
                            //    UDPRN = objPostalAddress.UDPRN ?? default(int),
                            //    AmendmentType = objPostalAddressBatch.AmendmentType,
                            //    FileName = strFileName,
                            //    FileProcessing_TimeStamp = DateTime.UtcNow,
                            //    FileType = FileType.Paf.ToString(),
                            //    ErrorMessage = Constants.PAFErrorMessageForUnmatchedDeliveryPointForUSRType,
                            //    SuccessFlag = false
                            //};

                            //fileProcessingLogDataService.LogFileException(objFileProcessingLog);
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
                            ErrorMessage = PAFErrorMessageForAddressTypeUSRNotFound,
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
                    await SaveDeliveryPointProcess(objPostalAddressBatch);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Save Delivery Point to the database
        /// </summary>
        /// <param name="objPostalAddress">Postal Address DTO</param>
        /// <param name="deliveryPoint">Delivery Point DTO</param>
        /// <returns>nothing</returns>
        private async Task SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress, DeliveryPointDTO deliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SaveDeliveryPointProcess"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        TASKNOTIFICATION,
                        NETWORKLINKDATAPROVIDER,
                        DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);
                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, TASKNOTIFICATION, TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, NETWORKLINKDATAPROVIDER, EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, DeliveryPointUseIndicator, DeliveryPointUseIndicatorPAF);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ReferenceDataCategoryNames.NetworkNodeType, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, NetworkNodeTypeRMGServiceNode, true);

                AddressLocationDTO addressLocationDTO = await postalAddressIntegrationService.GetAddressLocationByUDPRN((int)objPostalAddress.UDPRN);



                if (addressLocationDTO != null)
                {
                    DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
                        LocationXY = addressLocationDTO.LocationXY,
                    };

                    await postalAddressIntegrationService.InsertDeliveryPoint(deliveryPointDTO);

                    if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, TASKPAFACTION))
                    {
                        // update the notification if it exists.
                        await postalAddressIntegrationService.UpdateNotificationByUDPRN((int)objPostalAddress.UDPRN, TASKPAFACTION, NOTIFICATIONCLOSED);
                    }
                }
                else
                {
                    //var objTask = new NotificationDTO
                    //{
                    //    ID = Guid.NewGuid(),
                    //    NotificationType_GUID = tasktypeId,
                    //    NotificationPriority_GUID = null,
                    //    NotificationSource = Constants.TASKSOURCE,
                    //    Notification_Heading = Constants.TASKPAFACTION,
                    //    Notification_Message = AddressFields(objPostalAddress),
                    //    PostcodeDistrict = postCodeDistrict,
                    //    NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE),
                    //    NotificationActionLink = string.Format(Constants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
                    //};

                    if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, TASKPAFACTION))
                    {
                        string message = AddressFields(objPostalAddress);
                        await postalAddressIntegrationService.UpdateNotificationMessageByUDPRN((int)objPostalAddress.UDPRN, TASKPAFACTION, message);
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
        private string AddressFields(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.AddressFields"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

                string pafMessage = PAFTaskBodyPreText +
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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId);

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

        #endregion private methods
    }
}