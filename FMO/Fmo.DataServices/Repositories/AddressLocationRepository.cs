namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DTO;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using MappingConfiguration;

    /// <summary>
    /// AddressLocation Repository to interact with the AddressLocation entity
    /// </summary>
    public class AddressLocationRepository : RepositoryBase<AddressLocation, FMODBContext>, IAddressLocationRepository
    {
        public AddressLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Find AddressLocation by UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>boolean value</returns>
        public bool AddressLocationExists(int uDPRN)
        {
            try
            {
                if (DataContext.AddressLocations.Where(n => n.UDPRN == uDPRN).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="uDPRN"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>

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

        /// <summary>
        /// Add new address location to database.
        /// </summary>
        /// <param name="addressLocationDTO">AddressLocationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> SaveNewAddressLocation(AddressLocationDTO addressLocationDTO)
        {
            try
            {
                var addressLocationEntity = new AddressLocation();

                GenericMapper.Map(addressLocationDTO, addressLocationEntity);

                DataContext.AddressLocations.Add(addressLocationEntity);

                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}