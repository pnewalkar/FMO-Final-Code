using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// DataService to interact with Scenario entity and handle CRUD operations.
    /// </summary>
    public class ScenarioDataService : DataServiceBase<Scenario, UnitManagerDbContext>, IScenarioDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public ScenarioDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="locationID">Guid locationID</param>
        /// <returns>List</returns>
        public List<ScenarioDataDTO> GetRouteScenarios(Guid operationStateID, Guid locationID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetRouteScenarios);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRouteScenarios"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.ScenarioDataServiceMethodEntryEventId);

                var scenarios = (from Scenario in DataContext.Scenarios.AsNoTracking()
                                 join ScenarioStatus in DataContext.ScenarioStatus.AsNoTracking() on Scenario.ID equals ScenarioStatus.ScenarioID
                                 where ScenarioStatus.ScenarioStatusGUID == operationStateID && Scenario.LocationID == locationID
                                 select new ScenarioDataDTO
                                 {
                                     ScenarioName = Scenario.ScenarioName,
                                     ID = Scenario.ID
                                 }).OrderBy(n => n.ScenarioName);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.ScenarioDataServiceMethodExitEventId);
                return scenarios.ToList();
            }
        }
    }
}