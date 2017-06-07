using System;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;
using System.Data.Entity.Infrastructure;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.ResourceFile;
using System.Data.Entity;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// AddressLocation DataService to interact with the AddressLocation entity
    /// </summary>
    public class AddressLocationDataService : DataServiceBase<AddressLocation, RMDBContext>, IAddressLocationDataService
    {
        public AddressLocationDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }
        /// <summary>
        /// Find AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        public async Task<bool> AddressLocationExists(int udprn)
        {
            return await DataContext.AddressLocations.AsNoTracking().Where(n => n.UDPRN == udprn).AnyAsync();
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<AddressLocationDTO> GetAddressLocationByUDPRN(int udprn)
        {
            var objAddressLocation = await DataContext.AddressLocations.Where(n => n.UDPRN == udprn).SingleOrDefaultAsync();

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
                throw new DataAccessException(dbUpdateException, string.Format(ErrorMessageIds.Err_SqlAddException, string.Concat("Address Location for UDPRN:", addressLocationDTO.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorMessageIds.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new ServiceException(disposedException, ErrorMessageIds.Err_ObjectDisposedException);
            }
        }
    }
}