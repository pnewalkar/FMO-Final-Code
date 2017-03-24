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

        public bool SavePostalAddress(List<DTO.PostalAddress> lstPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                var refDataAddressType = refDataRepository.GetReferenceDataCategoryDetails("Postal Address Type");
                var refDataAddressStatus = refDataRepository.GetReferenceDataCategoryDetails("Postal Address Status");

                var lstPostalAddressEntities = GenericMapper.MapList<DTO.PostalAddress, Entity.PostalAddress>(lstPostalAddress);

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
