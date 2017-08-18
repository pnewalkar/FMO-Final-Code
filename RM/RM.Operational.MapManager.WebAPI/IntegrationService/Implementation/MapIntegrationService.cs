using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Operational.MapManager.WebAPI.Utils;

namespace RM.Operational.MapManager.WebAPI.IntegrationService
{
    public class MapIntegrationService : IMapIntegrationService
    {
        #region Property Declarations

        private string pdfGeneratorWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public MapIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.pdfGeneratorWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(MapManagerConstants.PDFGeneratorWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns>byte array</returns>
        public async Task<string> GenerateReportWithMap(string xml, string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GenerateReportWithMap"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(pdfGeneratorWebAPIName + "PDFReports", xml);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.MapManagerAPIPriority, LoggerTraceConstants.MapManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);
                return result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}