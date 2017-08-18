using System;
using System.IO;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Reporting.Pdf;

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    /// <summary>
    /// PDF Generator Business Service
    /// </summary>
    public class PDFGeneratorBusinessService : IPDFGeneratorBusinessService
    {
        /// <summary>
        /// PDF report folder path
        /// </summary>
        private string pdfReportFolderPath = string.Empty;



        /// <summary>
        /// Reference to the logging helper
        /// </summary>
        private ILoggingHelper loggingHelper = default(ILoggingHelper);





        /// <summary>
        /// Initializes a new instance of the <see cref="PDFGeneratorBusinessService" /> class
        /// </summary>
        /// <param name="pdfGeneratorBusinessService">PDF generator business service</param>
        /// <param name="configurationHelper">Configuration helper</param>
        /// <param name="loggingHelper">Logging helper</param>
        public PDFGeneratorBusinessService(IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (configurationHelper == null) { throw new ArgumentNullException(nameof(configurationHelper)); }
            if (loggingHelper == null) { throw new ArgumentNullException(nameof(loggingHelper)); }


            // Store the injected dependencies
            this.loggingHelper = loggingHelper;


            // Retrieve the configuration settings
            //
            // PDF report folder path
            const string PdfReportFolderPathConfigurationKey = "PdfReportFolderPath";
            this.pdfReportFolderPath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey).ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(this.pdfReportFolderPath)) { throw new System.Exception($"Configuration setting {PdfReportFolderPathConfigurationKey} must not be null or empty."); } // TODO update exception type
            if (!Directory.Exists(this.pdfReportFolderPath)) { throw new System.Exception($"Configuration setting {PdfReportFolderPathConfigurationKey} must point to a valid directory."); } // TODO update exception type
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
        public string CreatePdfReport(string reportXml)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(reportXml)) { throw new ArgumentException($"{nameof(reportXml)} must not be null or empty."); }


            using (this.loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(CreatePdfReport)}"))
            {
                string methodName = typeof(PDFGeneratorBusinessService) + "." + nameof(CreatePdfReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId);


                // Create the PDF report with a new file name
                string pdfFileName = CreatePdfReportFileName();
                string pdfFilePath = Path.Combine(this.pdfReportFolderPath, pdfFileName);
                bool created = PdfGenerator.CreatePDF(reportXml, pdfFilePath);
                if (!created)
                {
                    pdfFileName = string.Empty;
                    pdfFilePath = string.Empty;
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId);
                return pdfFileName;
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
        public string CreatePdfReport(string reportXml, string xsltFilePath)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(reportXml)) { throw new ArgumentException($"{nameof(reportXml)} must not be null or empty."); }
            if (string.IsNullOrWhiteSpace(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} must not be null or empty."); }
            if (!File.Exists(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} must point to a valid XSLT file."); }


            using (this.loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(CreatePdfReport)}"))
            {
                string methodName = typeof(PDFGeneratorBusinessService) + "." + nameof(CreatePdfReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId);


                // Create the PDF report with a new file name
                string pdfFileName = CreatePdfReportFileName();
                string pdfFilePath = Path.Combine(this.pdfReportFolderPath, pdfFileName);
                bool created = PdfGenerator.CreatePDF(reportXml, pdfFilePath, xsltFilePath);
                if (!created)
                {
                    pdfFileName = string.Empty;
                    pdfFilePath = string.Empty;
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId);
                return pdfFileName;
            }
        }



        /// <summary>
        /// Creates a new file name for a PDF report
        /// </summary>
        /// <returns>The PDF report file name</returns>
        private string CreatePdfReportFileName()
        {
            // Create a new PDF report file name
            string pdfFileName = Guid.NewGuid() + ".pdf";
            return pdfFileName;
        }



        /// <summary>
        /// Deletes the PDF report with the specified PDF document file name
        /// </summary>
        /// <param name="pdfFileName">The PDF document file name</param>
        public void DeletePdfReport(string pdfFileName)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(pdfFileName)) { throw new ArgumentException($"{nameof(pdfFileName)} must not be null or empty."); }
            if (pdfFileName.Contains("*")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain wildcards."); }
            if (pdfFileName.Contains("?")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain wildcards."); }
            if (pdfFileName.Contains("/")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }
            if (pdfFileName.Contains("\\")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }
            if (pdfFileName.Contains("..")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }


            using (this.loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(DeletePdfReport)}"))
            {
                string methodName = typeof(PDFGeneratorBusinessService) + "." + nameof(DeletePdfReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId);

                // If the PDF document file exists
                string pdfFilePath = Path.Combine(this.pdfReportFolderPath, pdfFileName);
                FileInfo pdfFile = new FileInfo(pdfFilePath);
                if (pdfFile.Exists)
                {
                    // If the PDF document file exists in the expected folder and has the expected name
                    //   This check helps to prevent attacks where the file name contains sequences of characters
                    //   that attempt to change folder
                    if (pdfFile.Directory.FullName == this.pdfReportFolderPath && pdfFile.Name == pdfFileName)
                    {
                        try
                        {
                            // Delete the file
                            pdfFile.Delete();
                        }
                        catch (IOException ex)
                        {
                            // Log the error but do not rethrow it because any undeleted PDF document files will be
                            //   deleted by an automated process
                            this.loggingHelper.Log(ex, System.Diagnostics.TraceEventType.Error);
                        }
                    }
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId);
            }
        }



        /// <summary>
        /// Gets the PDF report for the specified PDF document file name
        /// </summary>
        /// <param name="pdfFileName">The PDF document file name</param>
        /// <returns>The PDF report encoded as a byte array in a DTO</returns>
        public PdfFileDTO GetPdfReport(string pdfFileName)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(pdfFileName)) { throw new ArgumentException($"{nameof(pdfFileName)} must not be null or empty."); }
            if (pdfFileName.Contains("*")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain wildcards."); }
            if (pdfFileName.Contains("?")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain wildcards."); }
            if (pdfFileName.Contains("/")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }
            if (pdfFileName.Contains("\\")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }
            if (pdfFileName.Contains("..")) { throw new ArgumentException($"{nameof(pdfFileName)} must not contain folder delimiters (/, \\ or ..)."); }


            using (this.loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(GetPdfReport)}"))
            {
                string methodName = typeof(PDFGeneratorBusinessService) + "." + nameof(GetPdfReport);
                this.loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId);

                // Initialize the PDF report
                PdfFileDTO pdfReport = null;

                // If the PDF document file exists
                string pdfFilePath = Path.Combine(this.pdfReportFolderPath, pdfFileName);
                FileInfo pdfFile = new FileInfo(pdfFilePath);
                if (pdfFile.Exists)
                {
                    // If the PDF document file exists in the expected folder and has the expected name
                    //   This check helps to prevent attacks where the file name contains sequences of characters
                    //   that attempt to change folder
                    if (pdfFile.Directory.FullName == this.pdfReportFolderPath && pdfFile.Name == pdfFileName)
                    {
                        try
                        {
                            // Retrieve the PDF report document
                            byte[] pdfBytes = File.ReadAllBytes(pdfFile.FullName);
                            pdfReport = new PdfFileDTO { Data = pdfBytes, FileName = pdfFileName };
                        }
                        catch (IOException ex)
                        {
                            // Log the error but do not rethrow it because any undeleted PDF document files will be
                            //   deleted by an automated process
                            this.loggingHelper.Log(ex, System.Diagnostics.TraceEventType.Error);
                        }
                    }
                }

                this.loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId);
                return pdfReport;
            }
        }
    }
}