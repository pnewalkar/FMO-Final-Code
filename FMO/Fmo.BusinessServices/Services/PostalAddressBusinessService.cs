using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Entity = Fmo.Entities;
using Fmo.DTO;
using Fmo.MappingConfiguration;

namespace Fmo.BusinessServices.Services
{
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        IAddressRepository addressRepository = default(IAddressRepository);
        IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);

        public PostalAddressBusinessService(IAddressRepository _addressRepository, IReferenceDataCategoryRepository _refDataRepository)
        {
            this.addressRepository = _addressRepository;
            this.refDataRepository = _refDataRepository;
        }

        public bool SavePostalAddress(List<DTO.PostalAddressDTO> lstPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                int addressTypeId = refDataRepository.GetReferenceDataId("Postal Address Type","NYB");
                int addressStatusId = refDataRepository.GetReferenceDataId("Postal Address Status","L");

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

               
            }
            return saveFlag;
        }
    }
}
