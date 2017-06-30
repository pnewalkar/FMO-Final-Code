using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService;

namespace RM.DataManagement.DeliveryRoute.WebAPI.Controllers
{
    [Route("api/DeliveryRouteManager")]
    public class DeliveryRouteController : RMBaseController
    {
        private IDeliveryRouteBusinessService deliveryRouteLogBusinessService = default(IDeliveryRouteBusinessService);
        private ILoggingHelper logginghelper = default(ILoggingHelper);

        public DeliveryRouteController(IDeliveryRouteBusinessService deliveryRouteBusinessService, ILoggingHelper logginghelper)
        {
            this.logginghelper = logginghelper;
            this.deliveryRouteLogBusinessService = deliveryRouteBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Route
        /// </summary>
        /// <param name="operationStateID"> operationState ID</param>
        /// <param name="deliveryScenarioID">deliveryScenario ID</param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns>List</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroute/{operationStateID}/{deliveryScenarioID}/{fields}")]
        public IActionResult FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, string fields)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.FetchDeliveryRoute"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                var unitGuid = this.CurrentUserUnit;
                List<object> deliveryRoutesList = null;
                List<RouteDTO> deliveryRoutes = deliveryRouteLogBusinessService.FetchRoute(operationStateID, deliveryScenarioID, unitGuid);
                CreateSummaryObject<RouteDTO> createSummary = new CreateSummaryObject<RouteDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    deliveryRoutesList = createSummary.SummarisePropertiesForList(deliveryRoutes, fields);
                }

                logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(deliveryRoutesList);
            }
        }

        /// <summary>
        /// Fetch Delivery Route for Basic Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <returns>Task</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroutes/basic/{searchText}")]
        public async Task<IActionResult> FetchDeliveryRouteForBasicSearch(string searchText)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.FetchDeliveryRouteForBasicSearch"))
            {
                try
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.FetchDeliveryRouteForBasicSearch(searchText, CurrentUserUnit);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(deliveryRoutes);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        logginghelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Fetch Delivery Route For basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The total count of delivery route</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroutes/count/{searchText}")]
        public async Task<IActionResult> GetDeliveryRouteCount(string searchText)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.GetDeliveryRouteCount"))
            {
                try
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    int deliveryRouteCount = await deliveryRouteLogBusinessService.GetDeliveryRouteCount(searchText, CurrentUserUnit);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(deliveryRouteCount);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        logginghelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <returns>Task</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroutes/advance/{searchText}")]
        public async Task<IActionResult> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.FetchDeliveryRouteForAdvanceSearch"))
            {
                try
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);
                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.FetchDeliveryRouteForAdvanceSearch(searchText, CurrentUserUnit);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(deliveryRoutes);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        logginghelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Gets the delivery route details.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroute/routedetails/{deliveryRouteId}")]
        public async Task<IActionResult> GetDeliveryRouteDetailsForPdf(Guid deliveryRouteId)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.GetDeliveryRouteDetailsForPdf"))
            {
                try
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    var result = await deliveryRouteLogBusinessService.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, CurrentUserUnit);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(result);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        logginghelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Generates the delivery route log PDF.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpPost("deliveryroute/deliveryroutesummaries")]
        public async Task<IActionResult> GenerateRouteLog([FromBody]RouteDTO deliveryRouteDto)
        {
            using (logginghelper.RMTraceManager.StartTrace("WebService.GenerateRouteLog"))
            {
                try
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    var result = await deliveryRouteLogBusinessService.GenerateRouteLog(deliveryRouteDto, CurrentUserUnit);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(result);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        logginghelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Create mapping between delivery point and route
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>boolean</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [HttpGet]
        [Route("deliveryroute/deliverypointsequence/{deliveryRouteId}/{deliveryPointId}")]
        public async Task<IActionResult> CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            try
            {
                using (logginghelper.RMTraceManager.StartTrace("WebService.CreateBlockSequenceForDeliveryPoint"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    var result = await deliveryRouteLogBusinessService.CreateBlockSequenceForDeliveryPoint(deliveryRouteId, deliveryPointId);
                    logginghelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(result);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    logginghelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
    }
}