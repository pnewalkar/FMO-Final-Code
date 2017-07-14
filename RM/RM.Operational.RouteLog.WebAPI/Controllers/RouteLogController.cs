﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Operational.RouteLog.WebAPI.BusinessService;
using RM.Operational.RouteLog.WebAPI.Controllers.BaseController;

namespace RM.Operational.RouteLog.WebAPI.Controllers
{
    [Route("api/RouteLogManager")]
    public class RouteLogController : RMBaseController
    {
        private IRouteLogBusinessService routeLogBusinessService = default(IRouteLogBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.RouteLogAPIPriority;
        private int entryEventId = LoggerTraceConstants.RouteLogControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.RouteLogControllerMethodExitEventId;

        public RouteLogController(IRouteLogBusinessService deliveryRouteBusinessService, ILoggingHelper loggingHelper)
        {
            this.routeLogBusinessService = deliveryRouteBusinessService;
            this.loggingHelper = loggingHelper;
        }

        [HttpPost("routelogs")]
        public async Task<IActionResult> GenerateRouteLogSummaryReport([FromBody]RouteDTO deliveryRouteDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GenerateRouteLogSummaryReport"))
            {
                string methodName = typeof(RouteLogController) + "." + nameof(GenerateRouteLogSummaryReport);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    var pdfFilename = await routeLogBusinessService.GenerateRouteLog(deliveryRouteDto);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(pdfFilename);
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