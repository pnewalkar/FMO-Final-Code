using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.Controllers;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;

namespace RM.Operational.PDFGenerator.WebAPI.Controllers
{
    [Route("api/PDFGenerator")]
    public class PDFGeneratorController : RMBaseController
    {
        private IPDFGeneratorBusinessService pdfGeneratorBusinessService = default(IPDFGeneratorBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PDFGeneratorController(IPDFGeneratorBusinessService pdfGeneratorBusinessService, ILoggingHelper loggingHelper)
        {
            this.loggingHelper = loggingHelper;
            this.pdfGeneratorBusinessService = pdfGeneratorBusinessService;
        }

        [Route("PDFReports/{fileName}")]
        [HttpPost]
        public IActionResult CreateReport([FromBody]string xml, string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.CreateReport"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var pdfFile = pdfGeneratorBusinessService.CreateReport(xml, fileName);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(pdfFile);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    throw;
                }
            }
        }

        [Route("PDFReports/{pdfFileName}")]
        [HttpGet]
        public IActionResult GeneratePdfReport(string pdfFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GeneratePdfReport"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var pdfData = pdfGeneratorBusinessService.GeneratePdfReport(pdfFileName);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(pdfData);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    throw;
                }
            }
        }
    }
}