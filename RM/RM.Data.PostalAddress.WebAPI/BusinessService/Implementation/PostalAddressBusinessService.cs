using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
//using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
//using RM.CommonLibrary.EntityFramework.DTO;
//using RM.CommonLibrary.EntityFramework.DTO.Model;
//using RM.CommonLibrary.EntityFramework.DTO.UIDropdowns;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;
using RM.DataManagement.PostalAddress.WebAPI.DTO.UIDropdowns;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
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
        public async Task<PostalAddressDBDTO> GetPostalAddress(int? uDPRN)
        {
            return await addressDataService.GetPostalAddress(uDPRN);
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePostalAddressForNYB(List<PostalAddressDBDTO> lstPostalAddress, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SavePostalAddressForNYB"))
            {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodEntryEventId, LoggerTraceConstants.Title);

            bool isPostalAddressInserted = false;
            string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);

            try
            {
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        Constants.PostalAddressType,
                        Constants.PostalAddressStatus
                    };

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                Guid addressTypeId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(Constants.PostalAddressType, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid addressStatusId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(Constants.PostalAddressStatus, StringComparison.OrdinalIgnoreCase))
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
                            postalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(postalAddress.ID, addressStatusId));
                            postalAddress.AddressType_GUID = addressTypeId;
                            postalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(postalAddress.Postcode);
                            await addressDataService.SaveAddress(postalAddress, strFileName);
                        }

                        isPostalAddressInserted = await addressDataService.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                    }
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isPostalAddressProcessed = false;
                string postalAddressList = new JavaScriptSerializer() { MaxJsonLength = 50000000 }.Serialize(lstPostalAddress);
                try
                {
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(Constants.PostalAddressType).Result;

                    Guid addressTypeUSR = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Usr.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypePAF = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Paf.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();
                    Guid addressTypeNYB = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();

                    foreach (var item in lstPostalAddress)
                    {
                        loggingHelper.Log("record no " + lstPostalAddress.IndexOf(item)+" , Udprn :" + item.UDPRN, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                        await SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                    }

                    isPostalAddressProcessed = true;

                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePAFDetailsPriority, LoggerTraceConstants.SavePAFDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
        public async Task SaveDeliveryPointProcess(PostalAddressDBDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveDeliveryPointProcess"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // Call postalAddressIntegrationService to get reference data
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        Constants.TASKNOTIFICATION,
                        Constants.NETWORKLINKDATAPROVIDER,
                        Constants.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                Guid tasktypeId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(Constants.TASKNOTIFICATION, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(Constants.TASKACTION, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid locationProviderId = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(Constants.NETWORKLINKDATAPROVIDER, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(Constants.EXTERNAL, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid deliveryPointUseIndicator = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(Constants.DeliveryPointUseIndicator, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(Constants.DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid OperationalStatusGUIDLive = referenceDataCategoryList
                                .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(Constants.OperationalStatusGUIDLive, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();
                Guid NetworkNodeTypeRMGServiceNode = referenceDataCategoryList
                                .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkNodeType)
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(Constants.NetworkNodeTypeRMGServiceNode, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();

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
                        NotificationSource = Constants.TASKSOURCE,
                        Notification_Heading = Constants.TASKPAFACTION,
                        Notification_Message = AddressFields(objPostalAddress),
                        PostcodeDistrict = postCodeDistrict,
                        NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE),
                        NotificationActionLink = string.Format(Constants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
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

                        OperationalStatus_GUID = OperationalStatusGUIDLive,
                        NetworkNodeType_GUID = NetworkNodeTypeRMGServiceNode
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SaveDeliveryPointProcessPriority, LoggerTraceConstants.SaveDeliveryPointProcessBusinessMethodExitEventId, LoggerTraceConstants.Title);

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
            //using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressSearchDetails"))
            //{
            string methodName = MethodHelper.GetActualAsyncMethodName();
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

            try
            {
                List<string> listNames = new List<string> { FileType.Paf.ToString().ToUpper(), FileType.Nyb.ToString().ToUpper() };

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(ReferenceDataCategoryNames.PostalAddressType).Result;
                List<Guid> addresstypeIDs = referenceDataCategoryList.ReferenceDatas
                .Where(a => listNames.Contains(a.ReferenceDataValue))
                .Select(a => a.ID).ToList();
                List<Guid> postcodeGuids = await addressDataService.GetPostcodeGuids(searchText);
                List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postcodes = await postalAddressIntegrationService.GetPostcodes(unitGuid, postcodeGuids);
                return await addressDataService.GetPostalAddressSearchDetails(searchText, unitGuid, addresstypeIDs, postcodes);
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                throw;
            }
            finally
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressSearchDetailsPriority, LoggerTraceConstants.GetPostalAddressSearchDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
            }

            // }
        }

        /// <summary>
        /// Filter PostalAddress based on the post code
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        public async Task<PostalAddressDBDTO> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);

            try
            {
                List<BindingEntity> nybDetails = new List<BindingEntity>();
                PostalAddressDBDTO postalAddressDto = null;
                var postCodeGuids = addressDataService.GetSelectedPostcode(selectedItem).Result;
                var selectedPostcode = await postalAddressIntegrationService.GetPostcodes(unitGuid, postCodeGuids);
                var postalAddressDetails = await addressDataService.GetPostalAddressDetails(selectedItem, unitGuid, selectedPostcode);
                Guid nybAddressTypeId = postalAddressIntegrationService.GetReferenceDataGuId(Constants.PostalAddressType, FileType.Nyb.ToString()).Result;
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

                return postalAddressDto;
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                throw;
            }
            finally
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodExitEventId, LoggerTraceConstants.Title);
            }

            //}
        }

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDBDTO GetPostalAddressDetails(Guid id)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodEntryEventId, LoggerTraceConstants.Title);

            try
            {
                return addressDataService.GetPostalAddressDetails(id);
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
                throw;
            }
            finally
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId, LoggerTraceConstants.Title);
            }

            // }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        public string CheckForDuplicateNybRecords(PostalAddressDBDTO objPostalAddress)
        {
            try
            {
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(Constants.PostalAddressType).Result;
                Guid addressTypeNYB = referenceDataCategoryList.ReferenceDatas
                    .Where(a => a.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ID).FirstOrDefault();

                string postCode = string.Empty;
                postCode = addressDataService.CheckForDuplicateNybRecords(objPostalAddress, addressTypeNYB);
                return postCode;
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDBDTO objPostalAddress)
        {
            try
            {
                return addressDataService.CheckForDuplicateAddressWithDeliveryPoints(objPostalAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            try
            {
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus};

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(listNames).Result;

                Guid usrAddressTypeId = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.PostalAddressType)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == FileType.Usr.ToString().ToUpper()).Select(x => x.ID)
                    .SingleOrDefault();

                Guid liveAddressStatusId = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.PostalAddressStatus)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == Constants.LiveAddressStatus).Select(x => x.ID)
                    .SingleOrDefault();

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null)
                {
                    addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
                    addDeliveryPointDTO.PostalAddressDTO.PostCodeGUID = postalAddressIntegrationService.GetPostCodeID(addDeliveryPointDTO.PostalAddressDTO.Postcode).Result;
                    addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID = usrAddressTypeId;
                    addDeliveryPointDTO.PostalAddressDTO.PostalAddressStatus.Add(GetPostalAddressStatus(addDeliveryPointDTO.PostalAddressDTO.ID, liveAddressStatusId));
                }

                return addressDataService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO, liveAddressStatusId);
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
                throw ex;
            }
        }

        public async Task<List<PostalAddressDBDTO>> GetPostalAddresses(List<Guid> addressGuids)
        {
            try
            {
                var addressDetails = await addressDataService.GetPostalAddresses(addressGuids);
                return addressDetails;
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.SavePAFRecords"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                FileProcessingLogDTO objFileProcessingLog = null;
                Guid deliveryPointUseIndicatorPAF = Guid.Empty;
                Guid postCodeGuid = Guid.Empty;

                try
                {
                    // Construct New PostalAddressDTO
                    PostalAddressDBDTO objPostalAddress = new PostalAddressDBDTO
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
                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(Constants.PostalAddressStatus).Result;
                    var addressStatus_GUID = referenceDataCategoryList.ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(PostCodeStatus.Live.ToString(), StringComparison.OrdinalIgnoreCase)).Select(a => a.ID).FirstOrDefault();

                    // match if PostalAddress exists on UDPRN match
                    PostalAddressDBDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                    // match if PostalAddress exists on Address match
                    var objPostalAddressMatchedAddress = await addressDataService.GetPostalAddress(objPostalAddress);

                    // PAF process Logic
                    if (objPostalAddressMatchedUDPRN != null)
                    {
                        if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                        {
                            deliveryPointUseIndicatorPAF = postalAddressIntegrationService.GetReferenceDataSimpleLists(Constants.DeliveryPointUseIndicator).Result
                                            .ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(Constants.DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                            .Select(a => a.ID).FirstOrDefault();

                            objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                            objPostalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(objPostalAddress.Postcode);
                            objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                            if (await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF))
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
                                ErrorMessage = Constants.PAFErrorMessageForAddressTypeNYBNotFound,
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

                                deliveryPointUseIndicatorPAF = postalAddressIntegrationService.GetReferenceDataSimpleLists(Constants.DeliveryPointUseIndicator).Result
                                            .ReferenceDatas.Where(a => a.ReferenceDataValue.Equals(Constants.DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
                                            .Select(a => a.ID).FirstOrDefault();

                                objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                                // Update address and delivery point for USR records
                                await addressDataService.UpdateAddress(objPostalAddress, strFileName, deliveryPointUseIndicatorPAF);

                                await postalAddressIntegrationService.UpdateDeliveryPoint(objPostalAddress.ID, deliveryPointUseIndicatorPAF);
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
                                    ErrorMessage = Constants.PAFErrorMessageForUnmatchedDeliveryPointForUSRType,
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
                                AmendmentType = objPostalAddressBatch.AmendmentType,
                                FileName = strFileName,
                                FileProcessing_TimeStamp = DateTime.UtcNow,
                                FileType = FileType.Paf.ToString(),
                                ErrorMessage = Constants.PAFErrorMessageForAddressTypeUSRNotFound,
                                SuccessFlag = false
                            };

                            fileProcessingLogDataService.LogFileException(objFileProcessingLog);
                        }
                    }
                    else
                    {
                        objPostalAddress.ID = Guid.NewGuid();
                        objPostalAddress.PostCodeGUID = await postalAddressIntegrationService.GetPostCodeID(objPostalAddress.Postcode);
                        objPostalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(objPostalAddress.ID, addressStatus_GUID));
                        await addressDataService.InsertAddress(objPostalAddress, strFileName);
                        await SaveDeliveryPointProcess(objPostalAddress);
                    }

                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
        private string AddressFields(PostalAddressDBDTO objPostalAddress)
        {
            return Constants.PAFTaskBodyPreText +
                        objPostalAddress.OrganisationName + Constants.Comma +
                        objPostalAddress.DepartmentName + Constants.Comma +
                        objPostalAddress.BuildingName + Constants.Comma +
                        objPostalAddress.BuildingNumber + Constants.Comma +
                        objPostalAddress.SubBuildingName + Constants.Comma +
                        objPostalAddress.Thoroughfare + Constants.Comma +
                        objPostalAddress.DependentThoroughfare + Constants.Comma +
                        objPostalAddress.DependentLocality + Constants.Comma +
                        objPostalAddress.DoubleDependentLocality + Constants.Comma +
                        objPostalAddress.PostTown + Constants.Comma +
                        objPostalAddress.Postcode;
        }

        /// <summary>
        /// Construct Postal Address Status for resp Postal Address
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns postal address DTO</returns>
        private PostalAddressStatusDTO GetPostalAddressStatus(Guid postalAddressGUID, Guid operationalStatusGUID)
        {
            return new PostalAddressStatusDTO
            {
                ID = Guid.NewGuid(),
                PostalAddressGUID = postalAddressGUID,
                OperationalStatusGUID = operationalStatusGUID,
                StartDateTime = DateTime.UtcNow,
                RowCreateDateTime = DateTime.UtcNow
            };
        }

        #endregion private methods
    }
}