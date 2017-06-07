using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.Controllers;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;
using System;
using System.Diagnostics;

namespace RM.Operational.PDFGenerator.WebAPI.Controllers
{
    [Route("api/PDFGenerator")]
    public class PDFGeneratorController : RMBaseController
    {
        private IPDFGeneratorBusinessService pdfGeneratorBusinessService = default(IPDFGeneratorBusinessService);
        private ILoggingHelper logginghelper = default(ILoggingHelper);
        public PDFGeneratorController(IPDFGeneratorBusinessService pdfGeneratorBusinessService, ILoggingHelper logginghelper)
        {
            this.logginghelper = logginghelper;
            this.pdfGeneratorBusinessService = pdfGeneratorBusinessService;
        }

        [Route("PDFReports/{fileName}")]
        [HttpPost]
        public IActionResult GenerateRouteLogSummaryReport([FromBody]string xml, string fileName)
        {
            try
            {
                var xslFo = pdfGeneratorBusinessService.GenerateRouteLogSummaryReport(xml, fileName);
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