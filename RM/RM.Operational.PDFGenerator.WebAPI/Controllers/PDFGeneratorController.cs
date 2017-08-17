using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.Controllers;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;

namespace RM.Operational.PDFGenerator.WebAPI.Controllers
{
    /// <summary>
    /// PDF Generator Controller
    /// </summary>
    [Route("api/PDFGenerator")]
    public class PDFGeneratorController : RMBaseController
    {
        /// <summary>
        /// Reference to the PDF generator business service
        /// </summary>
        private IPDFGeneratorBusinessService pdfGeneratorBusinessService = default(IPDFGeneratorBusinessService);



        /// <summary>
        /// Reference to the logging helper
        /// </summary>
        private ILoggingHelper loggingHelper = default(ILoggingHelper);





        /// <summary>
        /// Initializes a new instance of the <see cref="PDFGeneratorController" /> class
        /// </summary>
        /// <param name="pdfGeneratorBusinessService">PDF generator business service</param>
        /// <param name="loggingHelper">Logging helper</param>
        public PDFGeneratorController(IPDFGeneratorBusinessService pdfGeneratorBusinessService, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (pdfGeneratorBusinessService == null) { throw new ArgumentNullException(nameof(pdfGeneratorBusinessService)); }
            if (loggingHelper == null) { throw new ArgumentNullException(nameof(loggingHelper)); }


            // Store the injected dependencies
            this.pdfGeneratorBusinessService = pdfGeneratorBusinessService;
            this.loggingHelper = loggingHelper;
        }





        /// <summary>
        /// Generates a PDF report from the specified report XML document using the default XSLT file and returns 
        ///   the file name of the generated PDF document
        /// The default XSLT file is FMO_PDFReport_Generic.xslt in RM.CommonLibrary.Reporting.Pdf
        /// The report XML document must be compliant with the schema defined in FMO_PDFReport_Generic.xsd in
        ///   RM.CommonLibrary.Reporting.Pdf. The RM.CommonLibrary.Reporting.Pdf.ReportFactoryHelper class
        ///   provides methods that create compliant elements and attributes, but the onus is on the developer
        ///   to verify that the report XML document is compliant
        /// </summary>
        /// <param name="reportXml">The XML report containing the report definition</param>
        /// <returns>The PDF document file name</returns>
        [Route("PDFReports")]
        [HttpPost]
        public IActionResult CreateReport([FromBody]string reportXml)
        {
            using (this.loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(CreateReport)}"))
            {
                string methodName = typeof(PDFGeneratorController) + "." + nameof(CreateReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodEntryEventId);

                // Initialize the PDF document file name
                string pdfFileName = string.Empty;

                try
                {
                    // Create the PDF report from the report XML
                    pdfFileName = this.pdfGeneratorBusinessService.CreatePdfReport(reportXml);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    throw;
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodExitEventId);
                return this.Ok(pdfFileName);
            }
        }



        /// <summary>
        /// Generates a PDF report from the specified report XML document using the specified XSLT file and returns
        ///   the file name of the generated PDF document
        /// The specified XSLT file must generate valid XSLFO output when applied to the report XML document
        /// </summary>
        /// <param name="reportXml">The XML report containing the report definition</param>
        /// <param name="xsltFilePath">The full path to the XSLT file</param>
        /// <returns>The PDF document file name</returns>
        [Route("PDFReports/{xsltFilePath}")]
        [HttpPost]
        public IActionResult CreateReport([FromBody]string reportXml, string xsltFilePath)
        {
            using (this.loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(CreateReport)}"))
            {
                string methodName = typeof(PDFGeneratorController) + "." + nameof(CreateReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodEntryEventId);

                // Initialize the PDF report file name
                string pdfFileName = string.Empty;

                try
                {
                    // Create the PDF report from the report XML using the specified XSLT file
                    pdfFileName = this.pdfGeneratorBusinessService.CreatePdfReport(reportXml, xsltFilePath);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    throw;
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodExitEventId);
                return this.Ok(pdfFileName);
            }
        }



        /// <summary>
        /// Gets the PDF report for the specified PDF document file name
        /// </summary>
        /// <param name="pdfFileName">The PDF document file name</param>
        /// <returns>The PDF report encoded as a byte array in a DTO</returns>
        [Route("PDFReports/{pdfFileName}")]
        [HttpGet]
        public IActionResult GetPdfReport(string pdfFileName)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(pdfFileName)) { throw new ArgumentException($"{nameof(pdfFileName)} must not be null or empty."); }


            using (this.loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(GetPdfReport)}"))
            {
                string methodName = typeof(PDFGeneratorController) + "." + nameof(GetPdfReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodEntryEventId);

                // Initialize the PDF report
                PdfFileDTO pdfReport = null;

                try
                {
                    // Get the PDF report for the specified PDF report file name
                    pdfReport = this.pdfGeneratorBusinessService.GetPdfReport(pdfFileName);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    throw;
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorControllerMethodExitEventId);
                if (pdfReport != null)
                {
                    return this.Ok(pdfReport);
                }
                else
                {
                    // The PDF report could not be found
                    return this.NotFound(pdfReport);
                }
            }
        }
    }
}