using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.DataManagement.UnitManager.WebAPI.Entity;
using RM.DataManagement.UnitManager.WebAPI.DTO;
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

        ///// <summary>
        ///// Fetch the delivery units for user.
        ///// </summary>
        ///// <param name="userId">The user identifier.</param>
        ///// <returns>
        ///// List of <see cref="UnitLocationDTO"/>.
        ///// </returns>
        //public List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchDeliveryUnitsForUser"))
        //    {
        //        string methodName = MethodBase.GetCurrentMethod().Name;
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //        var result = (from location in DataContext.UnitLocations.AsNoTracking()
        //                      join userRoleUnit in DataContext.UserRoleUnits.AsNoTracking() on unitLocation.ID equals userRoleUnit.Unit_GUID
        //                      where userRoleUnit.User_GUID == userId && unitLocation.UnitBoundryPolygon != null
        //                      select new UnitLocationDTO
        //                      {
        //                          ID = unitLocation.ID,
        //                          UnitName = unitLocation.UnitName,
        //                          Area = (
        //                                  from postcodeDistrict in DataContext.PostcodeDistricts
        //                                  join postcodeSector in DataContext.PostcodeSectors on postcodeDistrict.ID equals postcodeSector.DistrictGUID
        //                                  join unitPostcodeSector in DataContext.UnitPostcodeSectors on postcodeSector.ID equals unitPostcodeSector.PostcodeSector_GUID
        //                                  where unitPostcodeSector.Unit_GUID == unitLocation.ID
        //                                  select postcodeDistrict.Area).FirstOrDefault() ?? "",
        //                          UnitAddressUDPRN = unitLocation.UnitAddressUDPRN,
        //                          UnitBoundryPolygon = unitLocation.UnitBoundryPolygon,
        //                          ExternalId = unitLocation.ExternalId
        //                      }).ToList();

        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //        return result;
        //    }
        //}

        ///// <summary>
        ///// Fetches unit Location type id for current user
        ///// </summary>
        ///// <returns>Guid</returns>
        //public Guid GetUnitLocationTypeId(Guid unitId)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUnitLocationTypeId"))
        //    {
        //        string methodName = MethodBase.GetCurrentMethod().Name;
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //        var result = DataContext.UnitLocations.Where(n => n.ID == unitId).SingleOrDefault().LocationType_GUID;
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //        return result.Value;
        //    }
        //}

        //public async Task<PostCodeDTO> GetSelectedPostcode(Guid postcodeGuid, Guid unitGuid)
        //{
        //    var result = await (from pc in DataContext.Postcodes.AsNoTracking()
        //                        join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
        //                        where pc.ID == postcodeGuid && ul.Unit_GUID == unitGuid
        //                        select pc).SingleOrDefaultAsync();

        //    return GenericMapper.Map<Postcode, PostCodeDTO>(result);

        //}

        //public UnitLocationDTO FetchUnitDetails(Guid unitGuid)
        //{
        //    UnitLocation location = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).SingleOrDefault();
        //    return GenericMapper.Map<UnitLocation, UnitLocationDTO>(location);
        //}

        //public async Task<List<PostCodeDTO>> GetPostCodes(List<Guid> postcodeGuids, Guid unitGuid)
        //{
        //    var result = await (from pc in DataContext.Postcodes.AsNoTracking()
        //                        join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
        //                        where postcodeGuids.Contains(pc.ID) && ul.Unit_GUID == unitGuid
        //                        select pc).ToListAsync();

        //    return GenericMapper.MapList<Postcode, PostCodeDTO>(result);

        //}
    }
}
