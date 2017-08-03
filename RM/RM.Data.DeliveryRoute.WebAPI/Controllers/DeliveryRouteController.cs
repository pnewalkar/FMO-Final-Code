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
        private IDeliveryRouteBusinessService deliveryRouteLogBusinessService = default(IDeliveryRouteBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryRouteController(IDeliveryRouteBusinessService deliveryRouteBusinessService, ILoggingHelper loggingHelper)
        {
            // Store  injected dependencies
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
        public async Task<IActionResult> GetScenarioRoutes(Guid scenarioId, string fields)
        {
            if (scenarioId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(scenarioId));
            }

            if (string.IsNullOrEmpty(fields))
            {
                throw new ArgumentNullException(nameof(fields));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetScenarioRoutes"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetScenarioRoutes);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);
                List<object> routes = null;
                var routedetails = await deliveryRouteLogBusinessService.GetScenarioRoutes(scenarioId);

                CreateSummaryObject<RouteDTO> createSummary = new CreateSummaryObject<RouteDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    routes = createSummary.SummarisePropertiesForList(routedetails, fields);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);

                return Ok(routes);
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
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRoutesForBasicSearch"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRoutesForBasicSearch);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.GetRoutesForBasicSearch(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
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
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteCount"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRouteCount);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                    int routeCount = await deliveryRouteLogBusinessService.GetRouteCount(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
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
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRoutesForAdvanceSearch"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRoutesForAdvanceSearch);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                    List<RouteDTO> deliveryRoutes = await deliveryRouteLogBusinessService.GetRoutesForAdvanceSearch(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
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
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteSummary"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRouteSummary);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                    RouteDTO route = await deliveryRouteLogBusinessService.GetRouteSummary(routeId);

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
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
            if (routeDetails == null)
            {
                throw new ArgumentNullException(nameof(routeDetails));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GenerateRouteLog"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GenerateRouteLog);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                    var routeSummary = await deliveryRouteLogBusinessService.GenerateRouteLog(routeDetails);

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
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

        /// <summary>
        /// Get route details specific to postcode
        /// </summary>
        /// <param name="postCodeUnit">Post code</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>List of routes</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [HttpGet("deliveryroute/postcode/{postcodeunit}/{fields}")]
        public async Task<IActionResult> GetPostcodeSpecificRoutes(string postcodeunit, string fields)
        {
            if (string.IsNullOrEmpty(postcodeunit))
            {
                throw new ArgumentNullException(nameof(postcodeunit));
            }

            if (string.IsNullOrEmpty(fields))
            {
                throw new ArgumentNullException(nameof(fields));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostcodeSpecificRoutes"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetPostcodeSpecificRoutes);
                try
                {
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);
                    List<object> routes = null;
                    var routedetails = await deliveryRouteLogBusinessService.GetPostcodeSpecificRoutes(postcodeunit, CurrentUserUnit);

                    CreateSummaryObject<RouteDTO> createSummary = new CreateSummaryObject<RouteDTO>();

                    if (!string.IsNullOrEmpty(fields))
                    {
                        routes = createSummary.SummarisePropertiesForList(routedetails, fields);
                    }

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
                    return Ok(routes);
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
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        [HttpPost("deliveryroute/deliverypoint/{routeId}/{deliveryPointId}")]
        public IActionResult SaveDeliveryPointRouteMapping(Guid routeId, Guid deliveryPointId)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            if (deliveryPointId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(deliveryPointId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.SaveDeliveryPointRouteMapping"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(SaveDeliveryPointRouteMapping);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                deliveryRouteLogBusinessService.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
                return Ok();
            }
        }

        /// <summary>
        /// Get route details mapped to delivery point
        /// </summary>
        /// <param name="deliveryPointId">Delivery Point Id</param>
        /// <returns>Route Details</returns>
        [HttpGet("deliveryroute/deliverypoint/{deliveryPointId}")]
        public async Task<IActionResult> GetRouteByDeliveryPoint(Guid deliveryPointId)
        {
            if (deliveryPointId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(deliveryPointId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteByDeliverypoint"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(GetRouteByDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                var route = await deliveryRouteLogBusinessService.GetRouteByDeliveryPoint(deliveryPointId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
                return Ok(route);
            }
        }

        /// <summary>
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointIds">List of Delivery point unique id</param>
        [HttpPost("deliveryroute/deliverypointrange/{routeId}")]
        public IActionResult SaveDeliveryPointRouteMappingForRange(Guid routeId, [FromBody]List<Guid> deliveryPointIds)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            if (deliveryPointIds == null || deliveryPointIds.Count == 0)
            {
                throw new ArgumentNullException(nameof(deliveryPointIds));
            }

            using (loggingHelper.RMTraceManager.StartTrace("WebService.SaveDeliveryPointRouteMappingForRange"))
            {
                string methodName = typeof(DeliveryRouteController) + "." + nameof(SaveDeliveryPointRouteMappingForRange);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);

                foreach (var deliveryPointId in deliveryPointIds)
                {
                    deliveryRouteLogBusinessService.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
                return Ok();
            }
        }
    }
}