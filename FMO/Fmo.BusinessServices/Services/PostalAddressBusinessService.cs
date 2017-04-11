using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using Entity = Fmo.Entities;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.Common;

namespace Fmo.BusinessServices.Services
{
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
            bool saveFlag = false;
            try
            {
                Guid addressTypeId = refDataRepository.GetReferenceDataId(Constants.Postal_Address_Type, FileType.Nyb.ToString());
                Guid addressStatusId = refDataRepository.GetReferenceDataId(Constants.Postal_Address_Status, PostCodeStatus.Live.GetDescription());
                List<int> lstUDPRNS = lstPostalAddress.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
                if (!lstUDPRNS.All(a => a == 0))
                {
                    foreach (var postalAddress in lstPostalAddress)
                    {
                        postalAddress.AddressStatus_GUID = addressStatusId;
                        postalAddress.AddressType_GUID = addressTypeId;
                        addressRepository.SaveAddress(postalAddress, strFileName);
                    }

                    saveFlag = addressRepository.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }

            return saveFlag;
        }

        /// <summary>
        /// Business rules for PAF details
        /// </summary>
        /// <param name="objPostalAddress">address DTO</param>
        /// <returns>returns true or false</returns>
        public bool SavePAFDetails(PostalAddressDTO objPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                // var postalAddressEntities = GenericMapper.Map<DTO.PostalAddressDTO, Entity.PostalAddress>(postalAddress);
                Guid addressTypeUSR = refDataRepository.GetReferenceDataId("Postal Address Type", "USR");
                Guid addressTypePAF = refDataRepository.GetReferenceDataId("Postal Address Type", "PAF");
                Guid addressTypeNYB = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");

                var objPostalAddressMatchedUDPRN = addressRepository.GetPostalAddress(objPostalAddress.UDPRN);
                var objPostalAddressMatchedAddress = addressRepository.GetPostalAddress(objPostalAddress);
                if (objPostalAddressMatchedUDPRN != null)
                {
                    if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                    {
                        objPostalAddress.AddressType_GUID = addressTypePAF;
                        objPostalAddress.AddressStatus_GUID = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        if (addressRepository.UpdateAddress(objPostalAddress, null)) // 2nd param FileName for db logging
                        {
                            SaveDeliveryPointProcess(objPostalAddress);
                        }
                        else
                        {
                            FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                            objFileProcessingLog.FileID = Guid.NewGuid();
                            objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                            objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                            objFileProcessingLog.FileName = null; // 2nd param FileName for db logging
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
                        objFileProcessingLog.FileName = null; // 2nd param FileName for db logging
                        objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                        objFileProcessingLog.FileType = FileType.Paf.ToString();
                        objFileProcessingLog.NatureOfError = "Address Type of the selected Postal Address record is not <NYB>";//"RFMO 258 : Scenerio 1b";
                        fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                    }
                }
                else if (objPostalAddressMatchedAddress != null)
                {
                    if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                    {
                        objPostalAddress.AddressType_GUID = addressTypePAF;
                        objPostalAddress.AddressStatus_GUID = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        addressRepository.UpdateAddress(objPostalAddress, null); // 2nd param FileName for db logging
                    }
                    else
                    {
                        FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                        objFileProcessingLog.FileID = Guid.NewGuid();
                        objFileProcessingLog.UDPRN = objPostalAddress.UDPRN ?? default(int);
                        objFileProcessingLog.AmendmentType = objPostalAddress.AmendmentType;
                        objFileProcessingLog.FileName = null; // 2nd param FileName for db logging
                        objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                        objFileProcessingLog.FileType = FileType.Paf.ToString();
                        objFileProcessingLog.NatureOfError = "RFMO 259 : Scenerio 2a";
                        fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                    }
                }
                else
                {
                    addressRepository.InsertAddress(objPostalAddress, string.Empty);
                    SaveDeliveryPointProcess(objPostalAddress);
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
        /// <param name="objPostalAddress"></param>
        public void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress)
        {
            try
            {
                var objDeliveryPoint = deliveryPointsRepository.GetDeliveryPointByUDPRN(objPostalAddress.UDPRN ?? 0);
                var objAddressLocation = addressLocationRepository.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? 0);
                if (objDeliveryPoint == null && objAddressLocation != null)
                {
                    if (objAddressLocation.UDPRN == objPostalAddress.UDPRN)
                    {
                        var newDeliveryPoint = new DeliveryPointDTO();
                        newDeliveryPoint.UDPRN = objAddressLocation.UDPRN;
                        newDeliveryPoint.Address_Id = objPostalAddress.Address_Id;
                        newDeliveryPoint.LocationXY = objAddressLocation.LocationXY;
                        newDeliveryPoint.Latitude = objAddressLocation.Latitude;
                        newDeliveryPoint.Longitude = objAddressLocation.Longitude;
                        newDeliveryPoint.LocationProvider = "E"; // Update in Enum
                        deliveryPointsRepository.InsertDeliveryPoint(newDeliveryPoint);
                    }
                    else
                    {
                        var newDeliveryPoint = new DeliveryPointDTO();
                        newDeliveryPoint.UDPRN = objAddressLocation.UDPRN;
                        deliveryPointsRepository.InsertDeliveryPoint(newDeliveryPoint);

                        // Create task
                        Guid tasktypeId = refDataRepository.GetReferenceDataId("Notification type", "Task type");
                        var objTask = new NotificationDTO();
                        objTask.NotificationType_GUID = tasktypeId;
                        objTask.NotificationSource = "Source";
                        objTask.Notification_Heading = "Position new DP";
                        objTask.Notification_Message = "Please position the DP " + "a";
                        objTask.PostcodeDistrict = objPostalAddress.Postcode;
                        objTask.NotificationDueDate = DateTime.Now;
                        objTask.NotificationActionLink = ""; // Unique refn link
                        notificationRepository.AddNewNotification(objTask);
                    }
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }
        }
    }
}