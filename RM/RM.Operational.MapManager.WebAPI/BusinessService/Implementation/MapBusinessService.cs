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
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Operational.MapManager.WebAPI.IntegrationService;
using RM.Operational.MapManager.WebAPI.Utils;
using RM.CommonLibrary.Reporting.Pdf;

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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string pdfFilename = string.Empty;
                if (printMapDTO != null)
                {
                    MapConfiguration configuration = new MapConfiguration();
                    configuration.OutputTo = GetOutputTo(printMapDTO.PdfSize, printMapDTO.PdfOrientation);
                    string caption = printMapDTO.MapTitle;
                    string source = printMapDTO.ImagePath;
                    string timestamp = "Date: " + printMapDTO.PrintTime; // TODO load from resource file
                    string scale = "Scale: " + printMapDTO.CurrentScale; // TODO load from resource file
                    string[] legalNotices = { printMapDTO.License };
                    XmlDocument mapXml = MapFactory.GetMap(caption, source, timestamp, scale, legalNotices, configuration); //GenerateXml(printMapDTO);
                    pdfFilename = await mapIntegrationService.GenerateReportWithMap(mapXml.InnerXml, xsltFilepath);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return pdfFilename;
            }
        }


        private ReportFactoryHelper.ReportOutputToOption GetOutputTo(string size, string orientation)
        {
            // Default to A4 portrait
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;

            // Determine the selected output to option
            size = (size + string.Empty).Trim().ToUpper();
            orientation = (orientation + string.Empty).Trim().ToUpper();
            if (orientation == "PORTRAIT")
            {
                switch (size)
                {
                    case "A0":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A0Portrait;
                        break;
                    case "A1":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A1Portrait;
                        break;
                    case "A2":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A2Portrait;
                        break;
                    case "A3":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A3Portrait;
                        break;
                    case "A4":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
                        break;
                }
            }
            else
            {
                Debug.Assert(orientation == "LANDSCAPE");
                switch (size)
                {
                    case "A0":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A0Landscape;
                        break;
                    case "A1":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A1Landscape;
                        break;
                    case "A2":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A2Landscape;
                        break;
                    case "A3":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A3Landscape;
                        break;
                    case "A4":
                        outputTo = ReportFactoryHelper.ReportOutputToOption.A4Landscape;
                        break;
                }
            }

            return outputTo;
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