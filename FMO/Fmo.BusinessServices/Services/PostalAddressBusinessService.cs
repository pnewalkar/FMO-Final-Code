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

namespace Fmo.BusinessServices.Services
{
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        private IAddressRepository addressRepository = default(IAddressRepository);
        private IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);

        public PostalAddressBusinessService(IAddressRepository _addressRepository,
            IReferenceDataCategoryRepository _refDataRepository,
            IDeliveryPointsRepository deliveryPointsRepository,
            IAddressLocationRepository addressLocationRepository)
        {
            this.addressRepository = _addressRepository;
            this.refDataRepository = _refDataRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.addressLocationRepository = addressLocationRepository;
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public bool SavePostalAddress(List<DTO.PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            bool saveFlag = false;
            Guid addressTypeId = refDataRepository.GetReferenceDataId(Constants.Postal_Address_Type, FileType.Nyb.ToString());
            Guid addressStatusId = refDataRepository.GetReferenceDataId(Constants.Postal_Address_Status, "Live");
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

            return saveFlag;
        }

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
                        addressRepository.UpdateAddress(objPostalAddress);// remove addressTypeNYB later

                        SaveDeliveryPointProcess(objPostalAddress);
                    }
                    else
                    {
                        // error log entry
                    }
                }
                else if (objPostalAddressMatchedAddress != null)
                {
                    if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                    {
                        objPostalAddress.AddressType_GUID = addressTypePAF;
                        objPostalAddress.AddressStatus_GUID = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        addressRepository.UpdateAddress(objPostalAddress);// removed addressTypeNYB later
                    }
                    else
                    {
                        // error log entry
                    }
                }
                else
                {
                    addressRepository.SaveAddress(objPostalAddress, string.Empty); // insert postal address
                    SaveDeliveryPointProcess(objPostalAddress);
                }
                /*
                // Match address on UDPRN
                if (addressRepository.GetPostalAddress(postalAddress.UDPRN) == 0)
                {
                    // Match address on Address Details
                    if (addressRepository.GetPostalAddress(postalAddress) != 0)
                    {
                        postalAddress.AddressType_Id = addressTypePAF;
                        postalAddress.AddressStatus_Id = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        addressRepository.UpdateAddress(postalAddress, addressTypeUSR);
                    }

                    if (true)
                    {
                    }
                }
                else
                {
                    // Scenerio 1a
                    addressRepository.UpdateAddress(postalAddress, addressTypeNYB);

                    // chk address record have resp. dp
                    if (deliveryPointsRepository.GetDeliveryPointByUDPRN(postalAddress.UDPRN ?? 0) == null)
                    {
                        var addressLocation = addressLocationRepository.GetAddressLocationByUDPRN(postalAddress.UDPRN ?? 0);
                        var deliveryPointDTO = new DeliveryPointDTO();
                        deliveryPointDTO.UDPRN = addressLocation.UDPRN;
                        if (addressLocation != null)
                        {
                            deliveryPointDTO.Address_Id = postalAddress.Address_Id;
                            deliveryPointDTO.LocationXY = addressLocation.LocationXY;
                            deliveryPointDTO.Latitude = addressLocation.Latitude;
                            deliveryPointDTO.Longitude = addressLocation.Longitude;
                            deliveryPointDTO.LocationProvider = "E"; // Update in Enum
                            deliveryPointsRepository.InsertDeliveryPoint(deliveryPointDTO);
                        }
                        else
                        {
                            deliveryPointsRepository.InsertDeliveryPoint(deliveryPointDTO);

                            // create task
                        }
                    }
                    else
                    {
                    }
                }*/
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
                    Guid tasktypeId = refDataRepository.GetReferenceDataId("Notification type", "Task type");
                    var objTask = new NotificationDTO();
                    objTask.NotificationType_GUID = tasktypeId;
                    objTask.NotificationSource = "Source";
                    objTask.Notification_Heading = "Position new DP";
                    objTask.Notification_Message = "Please position the DP " + "a";
                    objTask.PostcodeDistrict = objPostalAddress.Postcode;
                    objTask.NotificationDueDate = DateTime.Now;
                    objTask.NotificationActionLink = ""; // Unique refn link
                }
            }
        }
    }
}