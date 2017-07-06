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

                var result = (from unitLocation in DataContext.UnitLocations.AsNoTracking()
                              join userRoleUnit in DataContext.UserRoleUnits.AsNoTracking() on unitLocation.ID equals userRoleUnit.Unit_GUID
                              where userRoleUnit.User_GUID == userId && unitLocation.UnitBoundryPolygon != null
                              select new UnitLocationDTO
                              {
                                  ID = unitLocation.ID,
                                  UnitName = unitLocation.UnitName,
                                  Area = (
                                          from postcodeDistrict in DataContext.PostcodeDistricts
                                          join postcodeSector in DataContext.PostcodeSectors on postcodeDistrict.ID equals postcodeSector.DistrictGUID
                                          join unitPostcodeSector in DataContext.UnitPostcodeSectors on postcodeSector.ID equals unitPostcodeSector.PostcodeSector_GUID
                                          where unitPostcodeSector.Unit_GUID == unitLocation.ID
                                          select postcodeDistrict.Area).FirstOrDefault() ?? "",
                                  UnitAddressUDPRN = unitLocation.UnitAddressUDPRN,
                                  UnitBoundryPolygon = unitLocation.UnitBoundryPolygon,
                                  ExternalId = unitLocation.ExternalId
                              }).ToList();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitLocationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return result;
            }
        }

        /// <summary>
        /// Fetches unit Location type id for current user
        /// </summary>
        /// <returns>Guid</returns>
        public Guid GetUnitLocationTypeId(Guid unitId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUnitLocationTypeId"))
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