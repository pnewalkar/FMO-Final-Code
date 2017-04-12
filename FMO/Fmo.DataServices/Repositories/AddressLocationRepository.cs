namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Linq;
    using DTO;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using MappingConfiguration;

    public class AddressLocationRepository : RepositoryBase<AddressLocation, FMODBContext>, IAddressLocationRepository
    {
        public AddressLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public AddressLocationDTO GetAddressLocationByUDPRN(int uDPRN)
        {
            try
            {
                var objAddressLocation = DataContext.AddressLocations.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                // return context.Students.Find(id);
                return GenericMapper.Map<AddressLocation, AddressLocationDTO>(objAddressLocation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveNewAddressLocation(AddressLocationDTO addressLocationDTO)
        {
            try
            {
                var addressLocationEntity = new AddressLocation();

                GenericMapper.Map(addressLocationDTO, addressLocationEntity);

                

                DataContext.AddressLocations.Add(addressLocationEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}