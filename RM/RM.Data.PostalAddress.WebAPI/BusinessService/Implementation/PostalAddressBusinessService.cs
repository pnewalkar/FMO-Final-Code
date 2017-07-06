using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.EntityFramework.DTO.UIDropdowns;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
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
            return await addressDataService.GetPostalAddress(uDPRN);
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePostalAddressForNYB(List<PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SavePostalAddressForNYB"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isPostalAddressInserted = false;
                string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);

                try
                {
                    List<string> categoryNamesSimpleLists = new List<string>
                    {
                        PostalAddressType,
                        PostalAddressStatus
                    };

                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                    Guid addressTypeId = referenceDataCategoryList
                                    .Where(list => list.CategoryName.Equals(PostalAddressType, StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(list => list.ReferenceDatas)
                                    .Where(item => item.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.ID).SingleOrDefault();
                    Guid addressStatusId = referenceDataCategoryList
                                    .Where(list => list.CategoryName.Equals(PostalAddressStatus, StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(list => list.ReferenceDatas)
                                    .Where(item => item.ReferenceDataValue.Equals(PostCodeStatus.Live.GetDescription(), StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.ID).SingleOrDefault();

                    if (lstPostalAddress != null && lstPostalAddress.Count > 0)
                    {
                        List<int> lstUDPRNS = lstPostalAddress.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
                        if (!lstUDPRNS.All(a => a == 0))
                        {
                            foreach (var postalAddress in lstPostalAddress)
                            {
                                postalAddress.AddressStatus_GUID = addressStatusId;
                                postalAddress.AddressType_GUID = addressTypeId;
                                postalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(postalAddress.Postcode);
                                await addressDataService.SaveAddress(postalAddress, strFileName);
                            }

                            isPostalAddressInserted = await addressDataService.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                        }
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                    this.loggingHelper.Log(postalAddressList, TraceEventType.Information);
                    throw;
                }

                return isPostalAddressInserted;
            }
        }

        /// <summary>
        /// Business rules for PAF details
        /// </summary>
        /// <param name="lstPostalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SavePAFDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isPostalAddressProcessed = false;
                string postalAddressList = new JavaScriptSerializer() { MaxJsonLength = 5000000 }.Serialize(lstPostalAddress);
                try
                {
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressType).Result;

                    Guid addressTypeUSR = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Usr.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypePAF = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Paf.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypeNYB = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();

                    foreach (var item in lstPostalAddress)
                    {
                        await SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                    }

                    isPostalAddressProcessed = true;

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveDeliveryPointProcess"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // Call postalAddressIntegrationService to get reference data
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                       TASKNOTIFICATION,
                        NETWORKLINKDATAPROVIDER,
                        DeliveryPointUseIndicator
                    };
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                Guid tasktypeId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(TASKNOTIFICATION, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(TASKACTION, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid locationProviderId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(NETWORKLINKDATAPROVIDER, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(EXTERNAL, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid deliveryPointUseIndicator = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(DeliveryPointUseIndicator, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();

                // Search Address Location for Postal Address If found, Add delivery point as per Address
                // Location details Else, Add delivery point w/o Address location details and also add
                // task in notification
                var objAddressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? default(int)).Result;

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);

                if (objAddressLocation == null)
                {
                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        UDPRN = objPostalAddress.UDPRN,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);

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
                        UDPRN = objAddressLocation.UDPRN,
                        LocationXY = objAddressLocation.LocationXY,
                        Latitude = objAddressLocation.Lattitude,
                        Longitude = objAddressLocation.Longitude,
                        LocationProvider_GUID = locationProviderId,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressSearchDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> listNames = new List<string> { FileType.Paf.ToString().ToUpper(), FileType.Nyb.ToString().ToUpper() };

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(ReferenceDataCategoryNames.PostalAddressType).Result;
                List<Guid> addresstypeIDs = referenceDataCategoryList.ReferenceDatas
                .Where(a => listNames.Contains(a.ReferenceDataValue))
                .Select(a => a.ID).ToList();

                var postalAddress = await addressDataService.GetPostalAddressSearchDetails(searchText, unitGuid, addresstypeIDs);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddress;
            }
        }

        /// <summary>
        /// Filter PostalAddress based on the post code
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        public async Task<PostalAddressDTO> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                List<BindingEntity> nybDetails = new List<BindingEntity>();
                PostalAddressDTO postalAddressDto = null;
                var postalAddressDetails = await addressDataService.GetPostalAddressDetails(selectedItem, unitGuid);
                Guid nybAddressTypeId = postalAddressIntegrationService.GetReferenceDataGuId(PostalAddressType, FileType.Nyb.ToString()).Result;
                if (postalAddressDetails != null && postalAddressDetails.Count > 0)
                {
                    postalAddressDto = postalAddressDetails[0];
                    foreach (var postalAddress in postalAddressDetails)
                    {
                        if (postalAddress.AddressType_GUID == nybAddressTypeId)
                        {
                            string address = string.Join(",", Convert.ToString(postalAddress.BuildingNumber) ?? string.Empty, postalAddress.BuildingName, postalAddress.SubBuildingName);
                            string formattedAddress = Regex.Replace(address, ",+", ",").Trim(',');
                            nybDetails.Add(new BindingEntity { Value = postalAddress.ID, DisplayText = formattedAddress });
                        }
                    }

                    nybDetails.OrderBy(n => n.DisplayText);
                    nybDetails.Add(new BindingEntity { Value = Guid.Empty, DisplayText = "Not Shown" });
                    postalAddressDto.NybAddressDetails = nybDetails;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddressDto;
            }
        }

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                var postalAddressDto = addressDataService.GetPostalAddressDetails(id);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddressDto;
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        public string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CheckForDuplicateNybRecords"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressType).Result;
                Guid addressTypeNYB = referenceDataCategoryList.ReferenceDatas
                    .Where(a => a.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ID).FirstOrDefault();

                string postCode = string.Empty;
                postCode = addressDataService.CheckForDuplicateNybRecords(objPostalAddress, addressTypeNYB);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                bool isDuplicate = addressDataService.CheckForDuplicateAddressWithDeliveryPoints(objPostalAddress);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateAddressAndDeliveryPoint"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus };

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(listNames).Result;

                Guid usrAddressTypeId = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.PostalAddressType)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == FileType.Usr.ToString().ToUpper()).Select(x => x.ID)
                    .SingleOrDefault();

                Guid liveAddressStatusId = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.PostalAddressStatus)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == LiveAddressStatus).Select(x => x.ID)
                    .SingleOrDefault();

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null)
                {
                    addDeliveryPointDTO.PostalAddressDTO.PostCodeGUID = postalAddressIntegrationService.GetPostCodeID(addDeliveryPointDTO.PostalAddressDTO.Postcode).Result;
                    addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID = usrAddressTypeId;
                    addDeliveryPointDTO.PostalAddressDTO.AddressStatus_GUID = liveAddressStatusId;
                }

                CreateDeliveryPointModelDTO createDeliveryPointModelDTO = addressDataService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return createDeliveryPointModelDTO;
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
        private async Task SavePAFRecords(PostalAddressDTO objPostalAddress, Guid addressTypeUSR, Guid addressTypeNYB, Guid addressTypePAF, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SavePAFRecords"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                FileProcessingLogDTO objFileProcessingLog = null;
                Guid deliveryPointUseIndicatorPAF = Guid.Empty;
                Guid postCodeGuid = Guid.Empty;
                bool isNyb = false;

                try
                {
                    // Address type will be PAF only in case of Inserting and updating records
                    objPostalAddress.AddressType_GUID = addressTypePAF;
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressStatus).Result;
                    objPostalAddress.AddressStatus_GUID = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(PostCodeStatus.Live.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();

                    // match if PostalAddress exists on UDPRN match
                    PostalAddressDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                    // match if PostalAddress exists on Address match
                    var objPostalAddressMatchedAddress = await addressDataService.GetPostalAddress(objPostalAddress);

                    // PAF process Logic
                    if (objPostalAddressMatchedUDPRN != null)
                    {
                        if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                        {
                            deliveryPointUseIndicatorPAF = postalAddressIntegrationService.GetReferenceDataSimpleLists(DeliveryPointUseIndicator).Result
                                            .ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                            .Select(a => a.ID).FirstOrDefault();

                            isNyb = true;
                            objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                            objPostalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(objPostalAddress.Postcode);
                            if (await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF, isNyb))
                            {
                                // calling delivery point web api
                                var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByUDPRN(objPostalAddress.UDPRN ?? default(int));
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
                                AmendmentType = objPostalAddress.AmendmentType,
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
                            if (objDeliveryPoint != null)
                            {
                                objPostalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(objPostalAddress.Postcode);

                                deliveryPointUseIndicatorPAF = postalAddressIntegrationService.GetReferenceDataSimpleLists(DeliveryPointUseIndicator).Result
                                            .ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                            .Select(a => a.ID).FirstOrDefault();

                                // Update address and delivery point for USR records
                                await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF, isNyb);
                            }
                            else
                            {
                                objFileProcessingLog = new FileProcessingLogDTO
                                {
                                    FileID = Guid.NewGuid(),
                                    UDPRN = objPostalAddress.UDPRN ?? default(int),
                                    AmendmentType = objPostalAddress.AmendmentType,
                                    FileName = strFileName,
                                    FileProcessing_TimeStamp = DateTime.UtcNow,
                                    FileType = FileType.Paf.ToString(),
                                    ErrorMessage = PAFErrorMessageForUnmatchedDeliveryPointForUSRType,
                                    SuccessFlag = false
                                };

                                fileProcessingLogDataService.LogFileException(objFileProcessingLog);
                            }
                        }
                        else
                        {
                            objFileProcessingLog = new FileProcessingLogDTO
                            {
                                FileID = Guid.NewGuid(),
                                UDPRN = objPostalAddress.UDPRN ?? default(int),
                                AmendmentType = objPostalAddress.AmendmentType,
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
                        objPostalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(objPostalAddress.Postcode);
                        await addressDataService.InsertAddress(objPostalAddress, strFileName);
                        await SaveDeliveryPointProcess(objPostalAddress);
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                }
            }
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDTO objPostalAddress)
        {
            return PAFTaskBodyPreText +
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
        }

        #endregion private methods
    }
}