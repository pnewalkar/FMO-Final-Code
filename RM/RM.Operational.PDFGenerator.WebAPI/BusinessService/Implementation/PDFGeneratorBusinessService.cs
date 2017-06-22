using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using Fonet;
using Microsoft.Extensions.FileProviders;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.PDFGenerator.WebAPI.Utils;

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    public class PDFGeneratorBusinessService : IPDFGeneratorBusinessService
    {
        private string xsltFilepath = string.Empty;
        private string pdfFilepath = string.Empty;
        private IFileProvider fileProvider = default(IFileProvider);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public PDFGeneratorBusinessService(IFileProvider fileProvider, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.fileProvider = fileProvider;
            this.pdfFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PDFGeneratorConstants.PDFFileLoaction).ToString() : string.Empty;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PDFGeneratorConstants.XSLTFilePath).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Create pdf file and store the pdf file in the File server
        /// </summary>
        /// <param name="xml">xsl fo as string</param>
        /// <param name="fileName">XSLT filename</param>
        /// <returns>Pdf file name</returns>
        public string CreateReport(string xml, string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateReport"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                IDirectoryContents contents = fileProvider.GetDirectoryContents(string.Empty);
                IFileInfo fileInfo = fileProvider.GetFileInfo(xsltFilepath + fileName);
                using (StreamReader reader = new StreamReader(fileInfo.CreateReadStream()))
                {
                    string xsltFilePath = reader.ReadToEnd();
                    var xslTransformer = new XslCompiledTransform();
                    XmlReader xsltReader = XmlReader.Create(new StringReader(xsltFilePath));
                    XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
                    StringWriter strWriter = new StringWriter();
                    xslTransformer.Load(xsltReader);
                    xslTransformer.Transform(xmlReader, null, strWriter);
                    var generateRouteLogSummaryPdf = GenerateRouteLogSummaryPdf(strWriter.ToString());
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return generateRouteLogSummaryPdf;
                }
            }
        }

        /// <summary>
        /// Reads pdf file from the file server
        /// </summary>
        /// <param name="pdfFileName">pdf name </param>
        /// <returns>pdf as byte array</returns>
        public PdfFileDTO GeneratePdfReport(string pdfFilename)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GeneratePdfReport"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                byte[] pdfBytes = File.ReadAllBytes(pdfFilepath + pdfFilename);
                PdfFileDTO pdfFileDTO = new PdfFileDTO { Data = pdfBytes, FileName = pdfFilename };
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PDFGeneratorAPIPriority, LoggerTraceConstants.PDFGeneratorBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return pdfFileDTO;
            }
        }

        /// <summary>
        /// Create pdf usinf XSL Fo and FONET assembly
        /// </summary>
        /// <param name="xml">xsl fo</param>
        /// <returns>pdf file name</returns>
        private string GenerateRouteLogSummaryPdf(string xml)
        {
            string pdfFileName = Guid.NewGuid() + ".pdf";
            MemoryStream stream = new MemoryStream();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            FonetDriver driver = FonetDriver.Make();
            driver.Render(xmlDocument, stream);
            File.WriteAllBytes(pdfFilepath + pdfFileName, stream.ToArray());
            return pdfFileName;
        }
    }
}