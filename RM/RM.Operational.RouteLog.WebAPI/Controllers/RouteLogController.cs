using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.Operational.RouteLog.WebAPI.BusinessService;
using RM.Operational.RouteLog.WebAPI.Controllers.BaseController;
using System;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Operational.RouteLog.WebAPI.Controllers
{
    [Route("api/RouteLogManager")]
    public class RouteLogController : RMBaseController
    {
        private IRouteLogBusinessService routeLogBusinessService = default(IRouteLogBusinessService);
        private ILoggingHelper logginghelper = default(ILoggingHelper);

        public RouteLogController(IRouteLogBusinessService deliveryRouteBusinessService, ILoggingHelper logginghelper)
        {
            this.routeLogBusinessService = deliveryRouteBusinessService;
            this.logginghelper = logginghelper;
        }

        [HttpPost("routelogs")]
        public async Task<IActionResult> GenerateRouteLogSummaryReport([FromBody]DeliveryRouteDTO deliveryRouteDto)
        {
            try
            {
                var pdfFilename = await routeLogBusinessService.GenerateRouteLog(deliveryRouteDto);
                return Ok(pdfFilename);
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