using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;

namespace RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteDataService deliveryRouteDataService;
        private IScenarioDataService scenarioDataService;
        private IDeliveryRouteIntegrationService deliveryRouteIntegrationService;
        private IBlockSequenceDataService blockSequenceDataService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        // private IHttpHandler httpHandler;
        // private string ReferenceDataWebapiUri = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public DeliveryRouteBusinessService(IDeliveryRouteDataService deliveryRouteDataService, IScenarioDataService scenarioDataService, IDeliveryRouteIntegrationService deliveryRouteIntegrationService, IBlockSequenceDataService blockSequenceDataService, ILoggingHelper loggingHelper)
        {
            // this.httpHandler = httpHandler;
            this.deliveryRouteDataService = deliveryRouteDataService;
            this.scenarioDataService = scenarioDataService;
            this.deliveryRouteIntegrationService = deliveryRouteIntegrationService;
            this.blockSequenceDataService = blockSequenceDataService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch Route by passing operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// List
        /// </returns>
        public List<RouteDTO> FetchRoutes(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchRoutes"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryRouteDataService.FetchRoutes(operationStateID, deliveryScenarioID, userUnit, GetUnitNameByUnitId(userUnit));
            }
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryScenario"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var fetchScenario = scenarioDataService.FetchScenario(operationStateID, deliveryScenarioID);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchScenario;
            }
        }

        /// <summary>
        /// Fetch the Delivery Route for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<RouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryRouteForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                var fetchDeliveryRouteForBasicSearch = await deliveryRouteDataService.FetchDeliveryRouteForBasicSearch(searchText, userUnit, GetUnitNameByUnitId(userUnit)).ConfigureAwait(false);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchDeliveryRouteForBasicSearch;
            }
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryRouteCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var getDeliveryRouteCount = await deliveryRouteDataService.GetDeliveryRouteCount(searchText, userUnit, GetUnitNameByUnitId(userUnit)).ConfigureAwait(false);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getDeliveryRouteCount;
            }
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="RouteDTO"/>.
        /// </returns>
        public async Task<List<RouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryRouteForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var fetchDeliveryRouteForAdvanceSearch = await deliveryRouteDataService.FetchDeliveryRouteForAdvanceSearch(searchText, unitGuid, GetUnitNameByUnitId(unitGuid)).ConfigureAwait(false);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchDeliveryRouteForAdvanceSearch;
            }
        }

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>DeliveryRouteDTO</returns>
        public async Task<RouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryRouteDetailsforPdfGeneration"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> categoryNames = new List<string>
            {
                ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.DeliveryRouteMethodType
            };

                var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                var deliveryRouteDto =
                    await deliveryRouteDataService.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, referenceDataCategoryList, unitGuid);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return deliveryRouteDto;
            }
        }

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>byte[]</returns>
        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(RouteDTO deliveryRouteDto, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateRouteLog"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                Guid operationalObjectTypeForDp = deliveryRouteIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP).Result;
                var generateRouteLog = await deliveryRouteDataService.GenerateRouteLog(deliveryRouteDto, userUnit, operationalObjectTypeForDp);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return generateRouteLog;
            }
        }

        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateBlockSequenceForDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isBlockSequencInserted = false;
                List<string> categoryNames = new List<string> { ReferenceDataCategoryNames.OperationalObjectType };
                var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;
                Guid operationalObjectTypeForDp = referenceDataCategoryList
                  .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).Select(x => x.ID)
                  .SingleOrDefault();
                BlockSequenceDTO blockSequenceDTO = new BlockSequenceDTO { ID = Guid.NewGuid(), OperationalObjectType_GUID = operationalObjectTypeForDp, OperationalObject_GUID = deliveryPointId };
                isBlockSequencInserted = await blockSequenceDataService.AddBlockSequence(blockSequenceDTO, deliveryRouteId);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isBlockSequencInserted;
            }
        }

        /// <summary>
        /// Get current users Unit by passing unit ID
        /// </summary>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        private string GetUnitNameByUnitId(Guid userUnit)
        {
            string unitName = string.Empty;
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetUnitNameByUnitId"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> categoryNames = new List<string> { ReferenceDataCategoryNames.UnitLocationType };

                var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                var locationtypeId = deliveryRouteIntegrationService.GetUnitLocationTypeId(userUnit).Result;

                if (referenceDataCategoryList != null && referenceDataCategoryList.Count > 0)
                {
                    var referenceData = referenceDataCategoryList.SingleOrDefault().ReferenceDatas;
                    unitName = referenceData.Where(n => n.ID == locationtypeId).SingleOrDefault().ReferenceDataValue;
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return unitName;
            }
        }
    }
}