using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.DataManagement.UnitManager.WebAPI.Entity;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    public class UnitLocationDataService : DataServiceBase<Location, UnitManagerDbContext>//, IUnitLocationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public UnitLocationDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Gets the all delivery units for an user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO"/>.
        /// </returns>
        public List<UnitLocationDataDTO> GetDeliveryUnitsForUser(Guid userId, Guid postcodeAreaGUID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchDeliveryUnitsForUser"))
            {
                string methodName = typeof(UnitLocationDataService) + "." + nameof(GetDeliveryUnitsForUser);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);

                var unitLocation = (from postalAddressIdentifier in DataContext.PostalAddressIdentifiers.AsNoTracking()
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
                              }).ToList();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);
                return unitLocation;
            }
        }

        /// <summary>
        /// Gets postcodes details by postcodeGuids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <param name="postcodeSectorGUID"></param>
        /// <returns></returns>
        public async Task<List<PostCodeDataDTO>> GetPostCodeDetails(List<Guid> postcodeGuids, Guid postcodeSectorGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeDetails);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeDetails"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);

                var postcodeDetails = await (from pc in DataContext.Postcodes.AsNoTracking()
                                             join postcodeHierarchy in DataContext.PostcodeHierarchies.AsNoTracking() on pc.ID equals postcodeHierarchy.ID
                                             where postcodeHierarchy.PostcodeTypeGUID == postcodeSectorGUID && postcodeGuids.Contains(pc.ID)
                                             select new PostCodeDataDTO
                                             {
                                                 ID = pc.ID,
                                                 InwardCode = pc.InwardCode,
                                                 OutwardCode = pc.OutwardCode,
                                                 PostcodeUnit = pc.PostcodeUnit,
                                                 Sector = postcodeHierarchy.ParentPostcode
                                             }).ToListAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId);
                return postcodeDetails;
            }
        }
    }
}
