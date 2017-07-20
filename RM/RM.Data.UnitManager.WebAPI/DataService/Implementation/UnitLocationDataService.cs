using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// DataService to interact with Location entity and handle CRUD operations.
    /// </summary>
    public class UnitLocationDataService : DataServiceBase<Location, UnitManagerDbContext>, IUnitLocationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public UnitLocationDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            // Store injected dependencies
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Gets the all delivery units for an user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postcodeAreaGUID">The post code area unique identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO"/>.
        /// </returns>
        public async Task<IEnumerable<UnitLocationDataDTO>> GetDeliveryUnitsByUser(Guid userId, Guid postcodeAreaGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetDeliveryUnitsByUser);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryUnitsByUser"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);

                var unitLocation = await (from postalAddressIdentifier in DataContext.PostalAddressIdentifiers.AsNoTracking()
                                          join location in DataContext.Locations.AsNoTracking() on postalAddressIdentifier.ID equals location.ID
                                          join userRoleLocation in DataContext.UserRoleLocations.AsNoTracking() on postalAddressIdentifier.ID equals userRoleLocation.LocationID
                                          where userRoleLocation.UserID == userId
                                          select new UnitLocationDataDTO
                                          {
                                              LocationId = postalAddressIdentifier.ID,
                                              Name = postalAddressIdentifier.Name,
                                              Shape = location.Shape,
                                              Area = (
                                                      from postcodeHierarchy in DataContext.PostcodeHierarchies
                                                      where postcodeHierarchy.PostcodeTypeGUID == postcodeAreaGUID
                                                      select postcodeHierarchy.Postcode).FirstOrDefault() ?? string.Empty,
                                          }).ToListAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId);
                return unitLocation;
            }
        }

        /// <summary>
        /// Gets postcodes details by postcodeGuids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <param name="postcodeSectorGUID"></param>
        /// <returns></returns>
        public async Task<List<PostcodeDataDTO>> GetPostcodes(List<Guid> postcodeGuids, Guid postcodeSectorGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodes);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodes"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);

                var postcodeDetails = await (from pc in DataContext.Postcodes.AsNoTracking()
                                             join postcodeHierarchy in DataContext.PostcodeHierarchies.AsNoTracking() on pc.ID equals postcodeHierarchy.ID
                                             where postcodeHierarchy.PostcodeTypeGUID == postcodeSectorGUID && postcodeGuids.Contains(pc.ID)
                                             select new PostcodeDataDTO
                                             {
                                                 ID = pc.ID,
                                                 InwardCode = pc.InwardCode,
                                                 OutwardCode = pc.OutwardCode,
                                                 PostcodeUnit = pc.PostcodeUnit,
                                                 Sector = postcodeHierarchy.ParentPostcode
                                             }).ToListAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId);
                return postcodeDetails;
            }
        }
    }
}