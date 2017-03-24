using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Entity = Fmo.DataServices.Entities;
using Fmo.DTO;
using AutoMapper;

namespace Fmo.BusinessServices.Services
{
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        IAddressRepository addressRepository = default(IAddressRepository);

        public PostalAddressBusinessService(IAddressRepository _addressRepository)
        {
            this.addressRepository = _addressRepository;
        }

        public bool SavePostalAddress(List<DTO.PostalAddress> lstPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                Mapper.Initialize(config => { config.CreateMap<DTO.PostalAddress, Entity.PostalAddress>(); });
                var lstPostalAddressEntities = Mapper.Map<List<DTO.PostalAddress>, List<Entity.PostalAddress>>(lstPostalAddress);
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
