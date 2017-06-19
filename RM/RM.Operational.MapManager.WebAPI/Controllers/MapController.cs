using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.MapManager.WebAPI.Controllers;
using RM.Operational.MapManager.WebAPI.BusinessService;
using System.Threading.Tasks;

namespace RM.Operational.MapManager.WebAPI.Controllers
{
    [Route("api/MapManager")]
    public class MapController : RMBaseController
    {
        private IMapBusinessService mapGeneratorBusinessService = default(IMapBusinessService);
        private ILoggingHelper logginghelper = default(ILoggingHelper);

        public MapController(IMapBusinessService mapGeneratorBusinessService, ILoggingHelper logginghelper)
        {
            this.logginghelper = logginghelper;
            this.mapGeneratorBusinessService = mapGeneratorBusinessService;
        }

        [Route("MapImage")]
        [HttpPost]
        public IActionResult GenerateReportWithMap([FromBody]PrintMapDTO printMapDTO)
        {
            try
            {
                var result = mapGeneratorBusinessService.SaveImage(printMapDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logginghelper.Log(ex, TraceEventType.Error);
                throw;
            }
        }

        [Route("MapPDF")]
        [HttpPost]
        public async Task<IActionResult> GeneratePdf([FromBody]PrintMapDTO printMapDTO)
        {
            try
            {
                var result = await mapGeneratorBusinessService.GenerateMapPdfReport(printMapDTO);
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
}