using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using AutoMapper;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DataService
{
    /// <summary>
    /// AddressLocation DataService to interact with the AddressLocation entity
    /// </summary>
    public class AddressLocationDataService : DataServiceBase<AddressLocation, AddressLocationDBContext>, IAddressLocationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private const string USRNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";

        public AddressLocationDataService(IDatabaseFactory<AddressLocationDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            // Store injected dependencies
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Find AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        public async Task<bool> AddressLocationExists(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.AddressLocationExists"))
            {
                string methodName = typeof(AddressLocationDataService) + "." + nameof(AddressLocationExists);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                bool addressLocationExists = await DataContext.AddressLocations.AsNoTracking().Where(n => n.UDPRN == udprn).AnyAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return addressLocationExists;
            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<AddressLocationDataDTO> GetAddressLocationByUDPRN(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAddressLocationByUDPRN"))
            {
                string methodName = typeof(AddressLocationDataService) + "." + nameof(GetAddressLocationByUDPRN);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                var objAddressLocation = await DataContext.AddressLocations.Where(n => n.UDPRN == udprn).SingleOrDefaultAsync();
                ConfigureMapper();
                var getAddressLocationByUDPRN = Mapper.Map<AddressLocation, AddressLocationDataDTO>(objAddressLocation);
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return getAddressLocationByUDPRN;
            }
        }

        /// <summary>
        /// Add new address location to database.
        /// </summary>
        /// <param name="addressLocationDTO">AddressLocationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> SaveNewAddressLocation(AddressLocationDataDTO addressLocationDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.SaveNewAddressLocation"))
                {
                    string methodName = typeof(AddressLocationDataService) + "." + nameof(SaveNewAddressLocation);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                    var addressLocationEntity = new AddressLocation();

                    Mapper.Initialize(cfg => cfg.CreateMap<AddressLocationDataDTO, AddressLocation>());
                    addressLocationEntity = Mapper.Map<AddressLocationDataDTO, AddressLocation>(addressLocationDTO);

                    GenericMapper.Map(addressLocationDTO, addressLocationEntity);
                    DataContext.AddressLocations.Add(addressLocationEntity);
                    var saveNewAddressLocation = await DataContext.SaveChangesAsync();
                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                    return saveNewAddressLocation;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Address Location for UDPRN:", addressLocationDTO.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }
        }

        /// <summary>
        /// Get the Postal Address based on UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>Postal Address record</returns>
        public async Task<PostalAddressDataDTO> GetPostalAddressData(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressData"))
            {
                string methodName = typeof(AddressLocationDataService) + "." + nameof(GetPostalAddressData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                PostalAddress postalAddress = await DataContext.PostalAddresses.Include(d => d.DeliveryPoints).Where(p => p.UDPRN == udprn).FirstOrDefaultAsync();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddress, PostalAddressDataDTO>().MaxDepth(1);
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().MaxDepth(2);
                });

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
            }
        }

        /// <summary>
        /// Check if there are any notification for the given UDPRN and action
        /// </summary>
        /// <param name="udprn">UDPRN ID</param>
        /// <param name="action">action message to be updated</param>
        /// <returns>whether the notification exists or not</returns>
        public async Task<bool> CheckIfNotificationExists(int udprn, string action)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckIfNotificationExists"))
            {
                string methodName = typeof(AddressLocationDataService) + "." + nameof(CheckIfNotificationExists);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodEntryEventId);

                string notificationActionlink = string.Format(USRNOTIFICATIONLINK, udprn.ToString());
                bool notificationExists = await DataContext.Notifications.AsNoTracking()
                .AnyAsync(notific => notific.NotificationActionLink.Equals(notificationActionlink) &&
                                  notific.Notification_Heading.Trim().Equals(action));
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationDataServiceMethodExitEventId);
                return notificationExists;
            }
        }

        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AddressLocation, AddressLocationDataDTO>().MaxDepth(1);
            });

            Mapper.Configuration.CreateMapper();
        }
    }
}