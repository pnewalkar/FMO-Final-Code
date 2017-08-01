using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.BusinessService;
using RM.Operational.RouteLog.WebAPI.Controllers.BaseController;
using RM.Operational.RouteLog.WebAPI.DTO;

namespace RM.Operational.RouteLog.WebAPI.Controllers
{
    /// <summary>
    /// Route Log Controller
    /// </summary>
    [Route("api/RouteLogManager")]
    public class RouteLogController : RMBaseController
    {
        /// <summary>
        /// Reference to the Route Log business service
        /// </summary>
        private IRouteLogBusinessService routeLogBusinessService = default(IRouteLogBusinessService);
        


        /// <summary>
        /// Reference to the logging helper
        /// </summary>
        private ILoggingHelper loggingHelper = default(ILoggingHelper);



        

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteLogController" /> class
        /// </summary>
        /// <param name="routeLogBusinessService">Route log business service</param>
        /// <param name="loggingHelper">Logging helper</param>
        public RouteLogController(IRouteLogBusinessService routeLogBusinessService, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (routeLogBusinessService == null) throw new ArgumentNullException(nameof(routeLogBusinessService));
            if (loggingHelper == null) throw new ArgumentNullException(nameof(loggingHelper));


            // Store the injected dependencies
            this.routeLogBusinessService = routeLogBusinessService;
            this.loggingHelper = loggingHelper;
        }





        /// <summary>
        /// Generates a route log summary report for the specified delivery route and returns the file name
        ///   of the generated PDF document
        /// </summary>
        /// <param name="deliveryRoute">The delivery route</param>
        /// <returns>The PDF document file name</returns>
        [HttpPost("routelogs")]
        public async Task<IActionResult> GenerateRouteLog([FromBody]RouteDTO deliveryRoute)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(GenerateRouteLog)}"))
            {
                string methodName = typeof(RouteLogController) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogControllerMethodEntryEventId);

                // Initialize the PDF document file name
                string pdfFilename = string.Empty;

                try
                {
                    // Generate the route log summary report for the specified delivery route
                    pdfFilename = await routeLogBusinessService.GenerateRouteLog(deliveryRoute);
                }
                catch (AggregateException ex)
                {
                    foreach (var exception in ex.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ex.Flatten().InnerException;
                    throw realExceptions;
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogControllerMethodExitEventId);
                return Ok(pdfFilename);
            }
        }
    }
}