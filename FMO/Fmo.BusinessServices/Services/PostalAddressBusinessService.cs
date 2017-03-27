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
                var refDataAddressType = refDataRepository.GetReferenceDataCategoryDetails("Postal Address Type");
                var refDataAddressStatus = refDataRepository.GetReferenceDataCategoryDetails("Postal Address Status");

                var lstPostalAddressEntities = GenericMapper.MapList<DTO.PostalAddressDTO, Entity.PostalAddress>(lstPostalAddress);

                foreach (var addEntity in lstPostalAddressEntities)
                {
                    addressRepository.SaveAddress(addEntity);
                }

                saveFlag = true;
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }
    }
}