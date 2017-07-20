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
        public async Task<IEnumerable<UnitLocationDataDTO>> GetUnitsByUser(Guid userId, Guid postcodeAreaGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetUnitsByUser);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUnitsByUser"))
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
        /// Gets the all units for an user whose access level is above mail centre.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postcodeAreaGUID">The post code area unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO"/>.
        /// </returns>
        public async Task<IEnumerable<UnitLocationDataDTO>> GetUnitsByUserForNational(Guid userId, Guid postcodeAreaGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetUnitsByUserForNational);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUnitsByUserForNational()"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);

                var userUnitDetails = await (from r in DataContext.UserRoleLocations.AsNoTracking()
                                             join lr in DataContext.LocationReferenceDatas on r.LocationID equals lr.LocationID
                                             join l in DataContext.Locations on lr.LocationID equals l.ID
                                             where r.UserID == userId
                                             select new UnitLocationDataDTO
                                             {
                                                 Shape = l.Shape,
                                                 LocationId = lr.LocationID,
                                                 Area = (
                                                      from postcodeHierarchy in DataContext.PostcodeHierarchies
                                                      where postcodeHierarchy.PostcodeTypeGUID == postcodeAreaGUID
                                                      select postcodeHierarchy.Postcode
                                                      ).FirstOrDefault() ?? string.Empty
                                             }).ToListAsync();
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId);
                return userUnitDetails;
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