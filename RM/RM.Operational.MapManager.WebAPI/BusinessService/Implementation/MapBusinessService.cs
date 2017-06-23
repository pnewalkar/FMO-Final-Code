using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.MapManager.WebAPI.IntegrationService;
using RM.Operational.MapManager.WebAPI.Utils;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    public class MapBusinessService : IMapBusinessService
    {
        private string xsltFilepath = string.Empty;
        private string imagePath = string.Empty;
        private IMapIntegrationService mapIntegrationService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public MapBusinessService(IMapIntegrationService mapIntegrationService, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.mapIntegrationService = mapIntegrationService;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(MapManagerConstants.XSLTFilePath).ToString() : string.Empty;
            this.imagePath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(MapManagerConstants.ImagePath).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Method to retrieve map details
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>deliveryRouteDto</returns>
        public PrintMapDTO SaveImage(PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveImage"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                if (printMapDTO != null)
                {
                    printMapDTO.PrintTime = string.Format(MapManagerConstants.PrintMapDateTimeFormat, DateTime.Now);
                    SaveMapImage(printMapDTO);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return printMapDTO;
            }
        }

        /// <summary>
        /// generate Print Map pdf using xsl fo
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Pdf file name </returns>
        public async Task<string> GenerateMapPdfReport(PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateMapPdfReport"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string pdfFilename = string.Empty;
                if (printMapDTO != null)
                {
                    string pdfXml = GenerateXml(printMapDTO);
                    pdfFilename = await mapIntegrationService.GenerateReportWithMap(pdfXml, xsltFilepath);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return pdfFilename;
            }
        }

        /// <summary>
        /// Custom serialization of Map DTO
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Xml as string</returns>
        private string GenerateXml(PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateXml"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                XmlDocument document = new XmlDocument();
                XmlElement report = document.CreateElement(MapManagerConstants.Report);
                XmlElement pageHeader = document.CreateElement(MapManagerConstants.PageHeader);
                XmlElement pageFooter = document.CreateElement(MapManagerConstants.PageFooter);
                XmlElement content = document.CreateElement(MapManagerConstants.Content);
                XmlElement heading1 = document.CreateElement(MapManagerConstants.Heading1);
                XmlElement heading1CenterAligned = document.CreateElement(MapManagerConstants.Heading1CenterAligned);
                XmlElement image = document.CreateElement(MapManagerConstants.Image);
                XmlElement section = null;
                XmlElement sectionColumn = null;

                report.SetAttribute(MapManagerConstants.PdfOutPut, printMapDTO.PdfSize + printMapDTO.PdfOrientation);
                pageHeader.SetAttribute(MapManagerConstants.Caption, string.Empty);
                pageFooter.SetAttribute(MapManagerConstants.Caption, string.Empty);
                pageFooter.SetAttribute(MapManagerConstants.PageNumber, string.Empty);
                report.AppendChild(pageHeader);
                report.AppendChild(pageFooter);
                report.AppendChild(content);

                // Section 1 Header
                section = document.CreateElement(MapManagerConstants.Section);

                // Section 1 Header 1
                sectionColumn = document.CreateElement(MapManagerConstants.SectionColumn);
                sectionColumn.SetAttribute(MapManagerConstants.Width, "1");
                heading1CenterAligned.InnerText = printMapDTO.MapTitle;
                sectionColumn.AppendChild(heading1CenterAligned);
                section.AppendChild(sectionColumn);
                content.AppendChild(section);

                // Section 2
                section = document.CreateElement(MapManagerConstants.Section);

                // Section 2 columns 1 i.e Table 1
                sectionColumn = document.CreateElement(MapManagerConstants.SectionColumn);
                sectionColumn.SetAttribute(MapManagerConstants.Width, "1");
                image.SetAttribute(MapManagerConstants.Source, printMapDTO.ImagePath);
                sectionColumn.AppendChild(image);
                section.AppendChild(sectionColumn);
                content.AppendChild(section);

                // Section 3
                section = document.CreateElement(MapManagerConstants.Section);
                sectionColumn = document.CreateElement(MapManagerConstants.SectionColumn);
                sectionColumn.SetAttribute(MapManagerConstants.Width, "1");
                sectionColumn.InnerText = "Date : " + printMapDTO.PrintTime;
                section.AppendChild(sectionColumn);

                sectionColumn = document.CreateElement(MapManagerConstants.SectionColumn);
                sectionColumn.SetAttribute(MapManagerConstants.Width, "1");
                sectionColumn.InnerText = "Scale : " + printMapDTO.CurrentScale;
                section.AppendChild(sectionColumn);
                content.AppendChild(section);

                // Section 4
                section = document.CreateElement(MapManagerConstants.Section);
                sectionColumn = document.CreateElement(MapManagerConstants.SectionColumn);
                sectionColumn.SetAttribute(MapManagerConstants.Width, "1");
                sectionColumn.InnerText = printMapDTO.License;
                section.AppendChild(sectionColumn);
                content.AppendChild(section);

                document.AppendChild(report);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return document.InnerXml;
            }
        }

        /// <summary>
        /// Method to save captured map image in png format.
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        private void SaveMapImage(PrintMapDTO printMapDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveMapImage"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                if (!string.IsNullOrEmpty(printMapDTO.EncodedString))
                {
                    string[] encodedStringArray = printMapDTO.EncodedString.Split(',');
                    string imageLocation = string.Empty;

                    if (encodedStringArray != null && encodedStringArray.Count() > 0)
                    {
                        imageLocation = imagePath + Guid.NewGuid() + ".png";
                        byte[] imageBytes = Convert.FromBase64String(encodedStringArray[1]);
                        File.WriteAllBytes(imageLocation, imageBytes);
                    }

                    printMapDTO.ImagePath = imageLocation;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
            }
        }
    }
}