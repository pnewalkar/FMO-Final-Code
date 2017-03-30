using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using System;
using System.Collections.Generic;
using Entity = Fmo.Entities;
using System.Linq;

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

                var lstPostalAddressEntities = GenericMapper.MapList<DTO.PostalAddressDTO, Entity.PostalAddress>(lstPostalAddress);

                foreach (var addEntity in lstPostalAddressEntities)
                {
                    addEntity.AddressStatus_Id = addressStatusId;
                    addEntity.AddressType_Id = addressTypeId;
                    addressRepository.SaveAddress(addEntity);
                }
                List<int> lstUDPRNS = lstPostalAddress.Select(n => n.UDPRN.Value).ToList();
                saveFlag = addressRepository.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
            }
            catch (Exception ex)
            {
            }
            return saveFlag;
        }

        public void SavePAFDetails(PostalAddressDTO postalAddress)
        {
            try
            {
                var postalAddressEntities = GenericMapper.Map<DTO.PostalAddressDTO, Entity.PostalAddress>(postalAddress);
                int addressTypeUSR = refDataRepository.GetReferenceDataId("Postal Address Type", "USR");
                int addressTypePAF = refDataRepository.GetReferenceDataId("Postal Address Type", "PAF");
                int addressTypeNYB = refDataRepository.GetReferenceDataId("Postal Address Type", "NYB");

                // Match address on UDPRN
                if (addressRepository.GetPostalAddress(postalAddressEntities.UDPRN) == 0)
                {
                    // Match address on Address Details
                    if (addressRepository.GetPostalAddress(postalAddressEntities) != 0)
                    {
                        postalAddressEntities.AddressType_Id = addressTypePAF;
                        postalAddressEntities.AddressStatus_Id = refDataRepository.GetReferenceDataId("Postal Address Status", "L");
                        addressRepository.UpdateAddress(postalAddressEntities, addressTypeUSR);
                    }
                }
                else
                {
                    addressRepository.UpdateAddress(postalAddressEntities, addressTypeNYB);
                    // chk address record have resp. dp
                    if (deliveryPointsRepository.GetDeliveryPointByUDPRN(postalAddressEntities.UDPRN ?? 0) == null)
                    {
                        if (addressLocationRepository.GetAddressLocationByUDPRN(postalAddressEntities.UDPRN ?? 0) != null)
                        { 
                            //create DP
                            //var deliveryPointEntities = GenericMapper.Map<DTO.DeliveryPointDTO, Entity.DeliveryPoint>();
                            //deliveryPointsRepository.InsertDeliveryPoint(deliveryPointEntities);
                            //update udprn of DP as UDPRN of adress locn

                            //update locan of DP as locan of adress locn
                            //update locan provider of DP as locan of adress locn
                        }
                        else
                        {
                            //create task
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