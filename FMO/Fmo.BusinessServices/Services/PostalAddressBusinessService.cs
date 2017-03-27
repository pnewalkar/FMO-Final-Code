using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.MappingConfiguration;
using Entity = Fmo.Entities;

namespace Fmo.BusinessServices.Services
{
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        private IAddressRepository addressRepository = default(IAddressRepository);
        private IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);

        public PostalAddressBusinessService(IAddressRepository addressRepository, IReferenceDataCategoryRepository refDataRepository)
        {
            this.addressRepository = addressRepository;
            this.refDataRepository = refDataRepository;
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

                saveFlag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saveFlag;
        }
    }
}