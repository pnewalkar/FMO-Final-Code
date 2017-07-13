using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService;
using RM.DataManagement.DeliveryRoute.WebAPI.DTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.Controllers
{
    [Route("api/DeliveryRouteManager")]
    public class DeliveryRouteController : RMBaseController
    {
        private int priority = LoggerTraceConstants.DeliveryRouteAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId;
        private IDeliveryRouteBusinessService deliveryRouteLogBusinessService = default(IDeliveryRouteBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryRouteController(IDeliveryRouteBusinessService deliveryRouteBusinessService, ILoggingHelper loggingHelper)
        {
            this.loggingHelper = loggingHelper;
            this.deliveryRouteLogBusinessService = deliveryRouteBusinessService;
        }

        /// <summary>
        /// Get Delivery Route specific to selected route
        /// </summary>
        /// <param name="scenarioId">Selected scenario ID from route log search </param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns>List of routes</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroute/{scenarioId}/{fields}")]
        public IActionResult GetScenarioRoutes(Guid scenarioId, string fields)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GetScenarioRoutes);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetScenarioRoutes"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                List<object> deliveryRoutesList = null;
                var routedetails = deliveryRouteLogBusinessService.GetScenarioRoutes(scenarioId);

                CreateSummaryObject<RouteDTO> createSummary = new CreateSummaryObject<RouteDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    deliveryRoutesList = createSummary.SummarisePropertiesForList(routedetails, fields);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(deliveryRoutesList);
            }
        }

        /// <summary>
        /// Get Delivery Route for Basic Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <returns>List of matched routes</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroutes/basic/{searchText}")]
        public async Task<IActionResult> GetRoutesForBasicSearch(string searchText)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRoutesForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRoutesForBasicSearch"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.GetRoutesForBasicSearch(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryRoutes);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
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
        public async Task<IActionResult> GetRouteCount(string searchText)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRouteCount);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteCount"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    int routeCount = await deliveryRouteLogBusinessService.GetRouteCount(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(routeCount);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Get routes for advance search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <returns>List of matched routes</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroutes/advance/{searchText}")]
        public async Task<IActionResult> GetRoutesForAdvanceSearch(string searchText)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRoutesForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRoutesForAdvanceSearch"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.GetRoutesForAdvanceSearch(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryRoutes);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
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
        /// <returns>Route details</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryroute/routedetails/{routeId}")]
        public async Task<IActionResult> GetRouteSummary(Guid routeId)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRouteSummary);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteSummary"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    RouteDTO route = await deliveryRouteLogBusinessService.GetRouteSummary(CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(route);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
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
        /// <returns>Route summary</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpPost("deliveryroute/deliveryroutesummaries")]
        public async Task<IActionResult> GenerateRouteLog([FromBody]RouteDTO routeDetails)
        {
            string methodName = typeof(DeliveryRouteController) + "." + nameof(GenerateRouteLog);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GenerateRouteLog"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var routeSummary = await deliveryRouteLogBusinessService.GenerateRouteLog(routeDetails);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(routeSummary);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }
    }
}