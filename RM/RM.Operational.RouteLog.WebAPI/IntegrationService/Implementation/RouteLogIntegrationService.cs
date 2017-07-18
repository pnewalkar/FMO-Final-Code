using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.DTO;
using RM.Operational.RouteLog.WebAPI.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    public class RouteLogIntegrationService : IRouteLogIntegrationService
    {
        #region Property Declarations

        private string deliveryRouteWebAPIName = string.Empty;
        private string pdfGeneratorWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.RouteLogAPIPriority;
        private int entryEventId = LoggerTraceConstants.RouteLogIntegrationServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.RouteLogIntegrationServiceMethodExitEventId;

        #endregion Property Declarations

        #region Constructor

        public RouteLogIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.deliveryRouteWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(RouteLogConstants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.pdfGeneratorWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(RouteLogConstants.PDFGeneratorWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRoute">deliveryRoute</param>
        /// <returns>deliveryRoute</returns>
        public async Task<RouteLogSummaryDTO> GenerateRouteLog(RouteDTO deliveryRoute)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GenerateRouteLog"))
            {
                string methodName = typeof(RouteLogIntegrationService) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryRouteWebAPIName + "deliveryroute/deliveryroutesummaries/", deliveryRoute);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var generateRouteLog = JsonConvert.DeserializeObject<RouteLogSummaryDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return generateRouteLog;
            }
        }

        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns>byte array</returns>
        public async Task<string> GenerateRouteLogSummaryReport(string xml, string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GenerateRouteLogSummaryReport"))
            {
                string methodName = typeof(RouteLogIntegrationService) + "." + nameof(GenerateRouteLogSummaryReport);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(pdfGeneratorWebAPIName + "PDFReports/" + fileName, xml);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var generateRouteLogSummaryReport = result.Content.ReadAsStringAsync().Result;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return generateRouteLogSummaryReport;
            }
        }
    }
}