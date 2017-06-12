using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.MapManager.WebAPI.Controllers;
using RM.Operational.MapManager.WebAPI.BusinessService;

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

        [Route("MapImages")]
        [HttpPost]
        public IActionResult GenerateReportWithMap([FromBody]PrintMapDTO printMapDTO)
        {
            try
            {
                var xslFo = mapGeneratorBusinessService.GenerateReportWithMap(printMapDTO);
                return Ok(xslFo);
            }
            catch (Exception ex)
            {
                this.logginghelper.Log(ex, TraceEventType.Error);
                throw;
            }
        }
    }
}