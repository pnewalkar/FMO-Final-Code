namespace Fmo.BusinessServices.Services
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        private IAddressRepository addressRepository = default(IAddressRepository);
        private IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);
        private IFileProcessingLogRepository fileProcessingLogRepository = default(IFileProcessingLogRepository);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostalAddressBusinessService(
            IAddressRepository addressRepository,
            IReferenceDataCategoryRepository refDataRepository,
            IDeliveryPointsRepository deliveryPointsRepository,
            IAddressLocationRepository addressLocationRepository,
            INotificationRepository notificationRepository,
            IFileProcessingLogRepository fileProcessingLogRepository,
            ILoggingHelper loggingHelper)
        {
            this.addressRepository = addressRepository;
            this.refDataRepository = refDataRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.addressLocationRepository = addressLocationRepository;
            this.notificationRepository = notificationRepository;
            this.fileProcessingLogRepository = fileProcessingLogRepository;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName)
        {
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
            bool saveFlag = false;
            try
            {
                Guid addressTypeUSR = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Usr.ToString());
                Guid addressTypePAF = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Paf.ToString());
                Guid addressTypeNYB = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());

                foreach (var item in lstPostalAddress)
                {
                    SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }

            return saveFlag;
        }

        /// <summary>
        /// private methods to save delivery point and Task for notification
        /// </summary>
        /// <param name="objPostalAddress">pass PostalAddreesDTO</param>
        public void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress)
        {
            try
            {
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
                    objTask.NotificationType_GUID = tasktypeId;
                    objTask.NotificationPriority_GUID = null;
                    objTask.NotificationSource = Constants.TASKSOURCE;
                    objTask.Notification_Heading = Constants.TASKPAFACTION;
                    objTask.Notification_Message = AddressFields(objPostalAddress);
                    objTask.PostcodeDistrict = postCodeDistrict;
                    objTask.NotificationDueDate = DateTime.Now.AddHours(24);
                    objTask.NotificationActionLink = string.Empty; // Unique refn link
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
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }
        }

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
            objPostalAddress.AddressType_GUID = addressTypePAF;
            objPostalAddress.AddressStatus_GUID = refDataRepository.GetReferenceDataId(Constants.PostalAddressStatus, PostCodeStatus.Live.GetDescription());
            var objPostalAddressMatchedUDPRN = addressRepository.GetPostalAddress(objPostalAddress.UDPRN);
            var objPostalAddressMatchedAddress = addressRepository.GetPostalAddress(objPostalAddress);
            if (objPostalAddressMatchedUDPRN != null)
            {
                if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                {
                    if (addressRepository.UpdateAddress(objPostalAddress, strFileName))
                    {
                        var objDeliveryPoint = deliveryPointsRepository.GetDeliveryPointByUDPRN(objPostalAddress.UDPRN ?? 0);
                        if (objDeliveryPoint == null)
                        {
                            SaveDeliveryPointProcess(objPostalAddress);
                        }
                    }
                    else
                    {
                        FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                        objFileProcessingLog.FileID = Guid.NewGuid();
                        objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                        objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                        objFileProcessingLog.FileName = strFileName;
                        objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                        objFileProcessingLog.FileType = FileType.Paf.ToString();
                        objFileProcessingLog.NatureOfError = "Postal Address for Selected UDPRN not updated";
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
                    objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                    objFileProcessingLog.FileType = FileType.Paf.ToString();
                    objFileProcessingLog.NatureOfError = "Address Type of the selected Postal Address record is not <NYB>";
                    fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                }
            }
            else if (objPostalAddressMatchedAddress != null)
            {
                if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                {
                    addressRepository.UpdateAddress(objPostalAddress, strFileName);
                }
                else
                {
                    FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                    objFileProcessingLog.FileID = Guid.NewGuid();
                    objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                    objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                    objFileProcessingLog.FileName = strFileName;
                    objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                    objFileProcessingLog.FileType = FileType.Paf.ToString();
                    objFileProcessingLog.NatureOfError = "Address Type of the selected Postal Address record is not <USR>";
                    fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                }
            }
            else
            {
                objPostalAddress.ID = Guid.NewGuid();
                addressRepository.InsertAddress(objPostalAddress, string.Empty);
                SaveDeliveryPointProcess(objPostalAddress);
            }
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDTO objPostalAddress)
        {
            return "Please position the DP " +
                        objPostalAddress.OrganisationName + ", " +
                        objPostalAddress.DepartmentName + ", " +
                        objPostalAddress.BuildingName + ", " +
                        objPostalAddress.BuildingNumber + ", " +
                        objPostalAddress.SubBuildingName + ", " +
                        objPostalAddress.Thoroughfare + ", " +
                        objPostalAddress.DependentThoroughfare + ", " +
                        objPostalAddress.DependentLocality + ", " +
                        objPostalAddress.DoubleDependentLocality + ", " +
                        objPostalAddress.PostTown + ", " +
                        objPostalAddress.Postcode;
        }

    }
}