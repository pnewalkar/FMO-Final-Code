using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class UnitLocationDataService : DataServiceBase<UnitLocation, RMDBContext>, IUnitLocationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public UnitLocationDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch the delivery unit.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>
        /// The <see cref="UnitLocationDTO"/>.
        /// </returns>
        public UnitLocationDTO FetchDeliveryUnit(Guid unitId)
        {
            var result = DataContext.UnitLocations.SingleOrDefault(x => x.ID == unitId && x.UnitBoundryPolygon != null);
            return GenericMapper.Map<UnitLocation, UnitLocationDTO>(result);
        }

        /// <summary>
        /// Fetch the delivery units for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO"/>.
        /// </returns>
        public List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchDeliveryUnitsForUser"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var result = (from a in DataContext.UnitLocations.AsNoTracking()
                              join b in DataContext.PostalAddresses.AsNoTracking() on a.UnitAddressUDPRN equals b.UDPRN
                              join c in DataContext.Postcodes.AsNoTracking() on b.PostCodeGUID equals c.ID
                              join d in DataContext.PostcodeSectors.AsNoTracking() on c.SectorGUID equals d.ID
                              join e in DataContext.PostcodeDistricts.AsNoTracking() on d.DistrictGUID equals e.ID
                              join f in DataContext.PostcodeAreas.AsNoTracking() on e.AreaGUID equals f.ID
                              join g in DataContext.UserRoleUnits.AsNoTracking() on a.ID equals g.Unit_GUID
                              where g.User_GUID == userId && a.UnitBoundryPolygon != null
                              select new UnitLocationDTO { ID = a.ID, UnitName = a.UnitName, Area = f.Area, UnitAddressUDPRN = a.UnitAddressUDPRN, UnitBoundryPolygon = a.UnitBoundryPolygon, ExternalId = a.ExternalId }).ToList();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return result;
            }
        }

        /// <summary>
        /// Fetches Location type id for current user
        /// </summary>
        /// <returns>Guid</returns>
        public Guid GetLocationTypeId(Guid unitId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetLocationTypeId"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var result = DataContext.UnitLocations.Where(n => n.ID == unitId).SingleOrDefault().LocationType_GUID;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return result.Value;
            }
        }
    }
}