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

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    public class PDFGeneratorBusinessService : IPDFGeneratorBusinessService
    {
        private const string XSLTFilePath = "XSLTFilePath";
        private const string PDFFileLoaction = "PDFFileLoaction";

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
            this.pdfFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PDFFileLoaction).ToString() : string.Empty;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(XSLTFilePath).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

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