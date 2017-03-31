using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Fmo.Entities;

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

        public bool SavePostalAddress(List<DTO.PostalAddressDTO> lstPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                int addressTypeId = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");
                int addressStatusId = refDataRepository.GetReferenceDataId("Postal Address Status", "L");

                foreach (var addEntity in lstPostalAddress)
                {
                    addEntity.AddressStatus_Id = addressStatusId;
                    addEntity.AddressType_Id = addressTypeId;
                    addressRepository.SaveAddress(addEntity);
                }

                List<int> lstUDPRNS = lstPostalAddress.Select(n => n.UDPRN.Value).ToList();
                saveFlag = addressRepository.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }

        public void SavePAFDetails(PostalAddressDTO postalAddress)
        {
            try
            {
                // var postalAddressEntities = GenericMapper.Map<DTO.PostalAddressDTO, Entity.PostalAddress>(postalAddress);
                int addressTypeUSR = refDataRepository.GetReferenceDataId("Postal Address Type", "USR");
                int addressTypePAF = refDataRepository.GetReferenceDataId("Postal Address Type", "PAF");
                int addressTypeNYB = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");

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
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}