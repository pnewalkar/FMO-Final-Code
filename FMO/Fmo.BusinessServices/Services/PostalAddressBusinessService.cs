using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using Entity = Fmo.Entities;
using Fmo.Common.Enums;

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

        public PostalAddressBusinessService(IAddressRepository _addressRepository,
            IReferenceDataCategoryRepository _refDataRepository,
            IDeliveryPointsRepository deliveryPointsRepository,
            IAddressLocationRepository addressLocationRepository,
            INotificationRepository notificationRepository,
            IFileProcessingLogRepository fileProcessingLogRepository)
        {
            this.addressRepository = _addressRepository;
            this.refDataRepository = _refDataRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.addressLocationRepository = addressLocationRepository;
            this.notificationRepository = notificationRepository;
            this.fileProcessingLogRepository = fileProcessingLogRepository;
        }

        public bool SavePostalAddress(List<DTO.PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            bool saveFlag = false;
            int addressTypeId = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");
            int addressStatusId = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
            List<int> lstUDPRNS = lstPostalAddress.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
            if (!lstUDPRNS.All(a => a == 0))
            {
                foreach (var postalAddress in lstPostalAddress)
                {
                    postalAddress.AddressStatus_Id = addressStatusId;
                    postalAddress.AddressType_Id = addressTypeId;
                    addressRepository.SaveAddress(postalAddress, strFileName);
                }

                saveFlag = addressRepository.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
            }

            return saveFlag;
        }

        public bool SavePAFDetails(PostalAddressDTO objPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                // var postalAddressEntities = GenericMapper.Map<DTO.PostalAddressDTO, Entity.PostalAddress>(postalAddress);
                int addressTypeUSR = refDataRepository.GetReferenceDataId("Postal Address Type", "USR");
                int addressTypePAF = refDataRepository.GetReferenceDataId("Postal Address Type", "PAF");
                int addressTypeNYB = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");

                var objPostalAddressMatchedUDPRN = addressRepository.GetPostalAddress(objPostalAddress.UDPRN);
                var objPostalAddressMatchedAddress = addressRepository.GetPostalAddress(objPostalAddress);
                if (objPostalAddressMatchedUDPRN != null)
                {
                    if (objPostalAddressMatchedUDPRN.AddressType_Id == addressTypeNYB)
                    {
                        objPostalAddress.AddressType_Id = addressTypePAF;
                        objPostalAddress.AddressStatus_Id = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        addressRepository.UpdateAddress(objPostalAddress, null); // 2nd param FileName for db logging

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
                        objFileProcessingLog.NatureOfError = "RFMO 258 : Scenerio 1b";
                        fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                    }
                }
                else if (objPostalAddressMatchedAddress != null)
                {
                    if (objPostalAddressMatchedAddress.AddressType_Id == addressTypeUSR)
                    {
                        objPostalAddress.AddressType_Id = addressTypePAF;
                        objPostalAddress.AddressStatus_Id = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
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
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }

        public void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress)
        {
            var objDeliveryPoint = deliveryPointsRepository.GetDeliveryPointByUDPRN(objPostalAddress.UDPRN ?? 0);
            var objAddressLocation = addressLocationRepository.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? 0);
            if (objDeliveryPoint == null)
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
                    int tasktypeId = refDataRepository.GetReferenceDataId("Notification type", "Task type");
                    var objTask = new NotificationDTO();
                    objTask.NotificationType_Id = tasktypeId;
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
    }
}