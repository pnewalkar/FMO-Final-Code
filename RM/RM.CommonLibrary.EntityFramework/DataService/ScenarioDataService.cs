using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class ScenarioDataService : DataServiceBase<Scenario, RMDBContext>, IScenarioDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public ScenarioDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch the Delivery Scenario by passing the operationstateID and deliveryUnitID.
        /// </summary>
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="deliveryUnitID">Guid deliveryUnitID</param>
        /// <returns>List</returns>
        public List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchScenario"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.ScenarioDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                IEnumerable<Scenario> result = DataContext.Scenarios.AsNoTracking().ToList().Where(x => x.OperationalState_GUID == operationStateID && x.Unit_GUID == deliveryUnitID);
                var fetchScenario = GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.ScenarioDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchScenario;
            }
        }
    }
}