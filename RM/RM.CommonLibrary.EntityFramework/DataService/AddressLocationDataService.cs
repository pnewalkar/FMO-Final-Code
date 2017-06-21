using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.ResourceFile;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// AddressLocation DataService to interact with the AddressLocation entity
    /// </summary>
    public class AddressLocationDataService : DataServiceBase<AddressLocation, RMDBContext>, IAddressLocationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public AddressLocationDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
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
                using (loggingHelper.RMTraceManager.StartTrace("DataService.SaveNewAddressLocation"))
                {
                    string method = MethodHelper.GetRealMethodFromAsyncMethod(MethodBase.GetCurrentMethod()).Name;
                    loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    var addressLocationEntity = new AddressLocation();

                    GenericMapper.Map(addressLocationDTO, addressLocationEntity);
                    DataContext.AddressLocations.Add(addressLocationEntity);
                    loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return await DataContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Address Location for UDPRN:", addressLocationDTO.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }
        }
    }
}