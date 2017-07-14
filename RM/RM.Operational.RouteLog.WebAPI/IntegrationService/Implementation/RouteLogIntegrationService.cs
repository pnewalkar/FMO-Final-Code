using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Operational.RouteLog.WebAPI.Utils;

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
        /// <param name="deliveryRouteDto">deliveryRouteDto</param>
        /// <returns>deliveryRouteDto</returns>
        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(RouteDTO deliveryRouteDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GenerateRouteLog"))
            {
                string methodName = typeof(RouteLogIntegrationService) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryRouteWebAPIName + "deliveryroute/deliveryroutesummaries/", deliveryRouteDto);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var generateRouteLog = JsonConvert.DeserializeObject<RouteLogSummaryModelDTO>(result.Content.ReadAsStringAsync().Result);
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