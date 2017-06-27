using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.MapManager.WebAPI.Controllers;
using RM.Operational.MapManager.WebAPI.BusinessService;

namespace RM.Operational.MapManager.WebAPI.Controllers
{
    [Route("api/MapManager")]
    public class MapController : RMBaseController
    {
        private IMapBusinessService mapGeneratorBusinessService = default(IMapBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public MapController(IMapBusinessService mapGeneratorBusinessService, ILoggingHelper loggingHelper)
        {
            this.loggingHelper = loggingHelper;
            this.mapGeneratorBusinessService = mapGeneratorBusinessService;
        }

        /// <summary>
        /// Api to save captured map image in png format
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Dto with saved image name</returns>
        [Route("MapImage")]
        [HttpPost]
        public IActionResult SaveMapImage([FromBody]PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.SaveMapImage"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                var result = mapGeneratorBusinessService.SaveImage(printMapDTO);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(result);
            }
        }

        /// <summary>
        /// Api to generate Print Map pdf using xsl fo
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Pdf file name </returns>
        [Route("MapPDF")]
        [HttpPost]
        public async Task<IActionResult> GeneratePdf([FromBody]PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GeneratePdf"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var result = await mapGeneratorBusinessService.GenerateMapPdfReport(printMapDTO);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(result);
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