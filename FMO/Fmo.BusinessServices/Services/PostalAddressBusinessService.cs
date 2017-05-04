using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        #region Property Declarations

        private IAddressRepository addressRepository = default(IAddressRepository);
        private IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);
        private IFileProcessingLogRepository fileProcessingLogRepository = default(IFileProcessingLogRepository);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private bool enableLogging = false;

        #endregion Property Declarations

        #region Constructor

        public PostalAddressBusinessService(
            IAddressRepository addressRepository,
            IReferenceDataCategoryRepository refDataRepository,
            IDeliveryPointsRepository deliveryPointsRepository,
            IAddressLocationRepository addressLocationRepository,
            INotificationRepository notificationRepository,
            IFileProcessingLogRepository fileProcessingLogRepository,
            ILoggingHelper loggingHelper,
            IConfigurationHelper configurationHelper)
        {
            this.addressRepository = addressRepository;
            this.refDataRepository = refDataRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.addressLocationRepository = addressLocationRepository;
            this.notificationRepository = notificationRepository;
            this.fileProcessingLogRepository = fileProcessingLogRepository;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.enableLogging = Convert.ToBoolean(configurationHelper.ReadAppSettingsConfigurationValues(Constants.EnableLogging));
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            bool isPostalAddressInserted = false;
            string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);

            try
            {
                Guid addressTypeId = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());
                Guid addressStatusId = refDataRepository.GetReferenceDataId(Constants.PostalAddressStatus, PostCodeStatus.Live.GetDescription());
                if (lstPostalAddress != null && lstPostalAddress.Count > 0)
                {
                    List<int> lstUDPRNS = lstPostalAddress.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
                    if (!lstUDPRNS.All(a => a == 0))
                    {
                        foreach (var postalAddress in lstPostalAddress)
                        {
                            postalAddress.AddressStatus_GUID = addressStatusId;
                            postalAddress.AddressType_GUID = addressTypeId;
                            addressRepository.SaveAddress(postalAddress, strFileName);
                        }

                        isPostalAddressInserted = addressRepository.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                    }
                }

                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogInfo(ex.ToString());
                this.loggingHelper.LogInfo(postalAddressList);
                throw;
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Business rules for PAF details
        /// </summary>
        /// <param name="lstPostalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        public bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            bool isPostalAddressProcessed = false;
            string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);
            try
            {
                Guid addressTypeUSR = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Usr.ToString());
                Guid addressTypePAF = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Paf.ToString());
                Guid addressTypeNYB = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());

                foreach (var item in lstPostalAddress)
                {
                    SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                }

                isPostalAddressProcessed = true;

                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                this.loggingHelper.LogInfo(postalAddressList);
                throw;
            }

            return isPostalAddressProcessed;
        }

        /// <summary>
        /// Method implementation to save delivery point and Task for notification for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">pass PostalAddreesDTO</param>
        public void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            var objAddressLocation = addressLocationRepository.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? 0);
            Guid tasktypeId = refDataRepository.GetReferenceDataId(Constants.TASKNOTIFICATION, Constants.TASKACTION);
            Guid locationProviderId = refDataRepository.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.EXTERNAL);
            string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);

            if (objAddressLocation == null)
            {
                var newDeliveryPoint = new DeliveryPointDTO();
                newDeliveryPoint.ID = Guid.NewGuid();
                newDeliveryPoint.Address_GUID = objPostalAddress.ID;
                newDeliveryPoint.UDPRN = objPostalAddress.UDPRN;
                deliveryPointsRepository.InsertDeliveryPoint(newDeliveryPoint);

                // Create task
                var objTask = new NotificationDTO();
                objTask.ID = Guid.NewGuid();
                objTask.Notification_Id = objPostalAddress.UDPRN ?? default(int);
                objTask.NotificationType_GUID = tasktypeId;
                objTask.NotificationPriority_GUID = null;
                objTask.NotificationSource = Constants.TASKSOURCE;
                objTask.Notification_Heading = Constants.TASKPAFACTION;
                objTask.Notification_Message = AddressFields(objPostalAddress);
                objTask.PostcodeDistrict = postCodeDistrict;
                objTask.NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE);
                objTask.NotificationActionLink = string.Format(Constants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN);
                notificationRepository.AddNewNotification(objTask).Wait();
            }
            else
            {
                var newDeliveryPoint = new DeliveryPointDTO();
                newDeliveryPoint.ID = Guid.NewGuid();
                newDeliveryPoint.Address_GUID = objPostalAddress.ID;
                newDeliveryPoint.UDPRN = objAddressLocation.UDPRN;
                newDeliveryPoint.Address_Id = objPostalAddress.Address_Id;
                newDeliveryPoint.LocationXY = objAddressLocation.LocationXY;
                newDeliveryPoint.Latitude = objAddressLocation.Lattitude;
                newDeliveryPoint.Longitude = objAddressLocation.Longitude;
                newDeliveryPoint.LocationProvider_GUID = locationProviderId;
                deliveryPointsRepository.InsertDeliveryPoint(newDeliveryPoint);
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
        }

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            try
            {
                List<string> searchdetails = new List<string>();
                List<PostalAddressDTO> lstAddress = await addressRepository.GetPostalAddressSearchDetails(searchText, unitGuid);
                if (lstAddress != null && lstAddress.Count > 0)
                {
                    var results = lstAddress.Select(n => new { Street = n.Thoroughfare, Postcode = n.Postcode }).Distinct().ToList();
                    if (results != null && results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            string searchitem = result.Street + ", " + result.Postcode;
                            string formattedResult = Regex.Replace(searchitem, ",+", ",").Trim(',');
                            searchdetails.Add(formattedResult);
                        }
                    }
                }

                searchdetails.Sort();
                return searchdetails;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// Filter PostalAddress based on the post code
        /// </summary>
        /// <param name="postCode">postCode</param>
        /// <returns>List of postcodes</returns>
        public async Task<PostalAddressDTO> GetPostalAddressDetails(string postCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            try
            {
                List<object> nybDetails = new List<object>();
                PostalAddressDTO postalAddressDto = null;
                var postalAddressDetails = await addressRepository.GetPostalAddressDetails(postCode);
                Guid nybAddressTypeId = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());
                if (postalAddressDetails != null && postalAddressDetails.Count > 0)
                {
                    postalAddressDto = postalAddressDetails[0];
                    foreach (var postalAddress in postalAddressDetails)
                    {
                        if (postalAddress.AddressType_GUID == nybAddressTypeId)
                        {
                            string address = string.Join(",", Convert.ToString(postalAddress.BuildingNumber) ?? string.Empty, postalAddress.BuildingName, postalAddress.SubBuildingName);
                            string formattedAddress = Regex.Replace(address, ",+", ",").Trim(',');
                            nybDetails.Add(new { Value = postalAddress.ID, DisplayText = formattedAddress });
                        }
                    }

                    nybDetails.Add(new { Value = Guid.Empty, DisplayText = Constants.NotShown });
                    postalAddressDto.NybAddressDetails = nybDetails;
                }

                return postalAddressDto;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid id)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            try
            {
                return addressRepository.GetPostalAddressDetails(id);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }

            return null;
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
        private void SavePAFRecords(PostalAddressDTO objPostalAddress, Guid addressTypeUSR, Guid addressTypeNYB, Guid addressTypePAF, string strFileName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            objPostalAddress.AddressType_GUID = addressTypePAF;
            objPostalAddress.AddressStatus_GUID = refDataRepository.GetReferenceDataId(Constants.PostalAddressStatus, PostCodeStatus.Live.GetDescription());
            var objPostalAddressMatchedUDPRN = addressRepository.GetPostalAddress(objPostalAddress.UDPRN);
            var objPostalAddressMatchedAddress = addressRepository.GetPostalAddress(objPostalAddress);
            if (objPostalAddressMatchedUDPRN != null)
            {
                if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                {
                    objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                    if (addressRepository.UpdateAddress(objPostalAddress, strFileName, Constants.PAFUPDATEFORNYB))
                    {
                        var objDeliveryPoint = deliveryPointsRepository.GetDeliveryPointByUDPRN(objPostalAddress.UDPRN ?? 0);
                        if (objDeliveryPoint == null)
                        {
                            SaveDeliveryPointProcess(objPostalAddress);
                        }
                    }
                }
                else
                {
                    FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                    objFileProcessingLog.FileID = Guid.NewGuid();
                    objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                    objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                    objFileProcessingLog.FileName = strFileName;
                    objFileProcessingLog.FileProcessing_TimeStamp = DateTime.UtcNow;
                    objFileProcessingLog.FileType = FileType.Paf.ToString();
                    objFileProcessingLog.NatureOfError = Constants.PAFErrorMessageForAddressTypeNYBNotFound;
                    fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                }
            }
            else if (objPostalAddressMatchedAddress != null)
            {
                if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                {
                    objPostalAddress.ID = objPostalAddressMatchedAddress.ID;
                    var objDeliveryPoint = deliveryPointsRepository.GetDeliveryPointByPostalAddress(objPostalAddress.ID);
                    if (objDeliveryPoint != null)
                    {
                        addressRepository.UpdateAddress(objPostalAddress, strFileName, Constants.PAFUPDATEFORUSR);
                        var isDeliveryPointUpdated = deliveryPointsRepository.UpdateDeliveryPointByAddressId(objPostalAddressMatchedAddress.ID, objPostalAddress.UDPRN ?? default(int));
                        if (!isDeliveryPointUpdated)
                        {
                            FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                            objFileProcessingLog.FileID = Guid.NewGuid();
                            objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                            objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                            objFileProcessingLog.FileName = strFileName;
                            objFileProcessingLog.FileProcessing_TimeStamp = DateTime.UtcNow;
                            objFileProcessingLog.FileType = FileType.Paf.ToString();
                            objFileProcessingLog.NatureOfError = Constants.PAFErrorMessageForMatchedDeliveryPointNotUpdatedForUSRType;
                            fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                        }
                    }
                    else
                    {
                        FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                        objFileProcessingLog.FileID = Guid.NewGuid();
                        objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                        objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                        objFileProcessingLog.FileName = strFileName;
                        objFileProcessingLog.FileProcessing_TimeStamp = DateTime.UtcNow;
                        objFileProcessingLog.FileType = FileType.Paf.ToString();
                        objFileProcessingLog.NatureOfError = Constants.PAFErrorMessageForUnmatchedDeliveryPointForUSRType;
                        fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                    }
                }
                else
                {
                    FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                    objFileProcessingLog.FileID = Guid.NewGuid();
                    objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                    objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                    objFileProcessingLog.FileName = strFileName;
                    objFileProcessingLog.FileProcessing_TimeStamp = DateTime.UtcNow;
                    objFileProcessingLog.FileType = FileType.Paf.ToString();
                    objFileProcessingLog.NatureOfError = Constants.PAFErrorMessageForAddressTypeUSRNotFound;
                    fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                }
            }
            else
            {
                objPostalAddress.ID = Guid.NewGuid();
                addressRepository.InsertAddress(objPostalAddress, strFileName);
                SaveDeliveryPointProcess(objPostalAddress);
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDTO objPostalAddress)
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

        #endregion private methods

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="separator">separator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string separator)
        {
            this.loggingHelper.LogInfo(methodName + separator + logMessage, this.enableLogging);
        }
    }
}