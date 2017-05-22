using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.Interface;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// AddressLocation Repository to interact with the AddressLocation entity
    /// </summary>
    public class AddressLocationRepository : RepositoryBase<AddressLocation, FMODBContext>, IAddressLocationRepository
    {
        private IExceptionHelper exceptionHelper = default(IExceptionHelper);

        public AddressLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory, IExceptionHelper exceptionHelper)
            : base(databaseFactory)
        {
            this.exceptionHelper = exceptionHelper;
        }

        /// <summary>
        /// Find AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        public bool AddressLocationExists(int udprn)
        {

            if (DataContext.AddressLocations.AsNoTracking().Where(n => n.UDPRN == udprn).Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public AddressLocationDTO GetAddressLocationByUDPRN(int udprn)
        {
            var objAddressLocation = DataContext.AddressLocations.Where(n => n.UDPRN == udprn).SingleOrDefault();

            return GenericMapper.Map<AddressLocation, AddressLocationDTO>(objAddressLocation);
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
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorMessageConstants.SqlAddExceptionMessage, string.Concat("Address Location for UDPRN:", addressLocationDTO.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                throw new InfrastructureException(notSupportedException, ErrorMessageConstants.NotSupportedExceptionMessage);
            }
            catch (ObjectDisposedException disposedException)
            {
                throw new ServiceException(disposedException, ErrorMessageConstants.ObjectDisposedExceptionMessage);
            }
        }
    }
}