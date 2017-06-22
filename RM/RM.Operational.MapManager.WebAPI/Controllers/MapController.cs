using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        /// <summary>
        /// Api to save captured map image in png format
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Dto with saved image name</returns>
        [Route("MapImage")]
        [HttpPost]
        public IActionResult SaveMapImage([FromBody]PrintMapDTO printMapDTO)
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

        /// <summary>
        /// Api to generate Print Map pdf using xsl fo
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Pdf file name </returns>
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