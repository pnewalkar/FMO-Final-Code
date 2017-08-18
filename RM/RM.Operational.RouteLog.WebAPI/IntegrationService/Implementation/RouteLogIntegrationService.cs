using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.DTO;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    /// <summary>
    /// Route Log Integration Service
    /// </summary>
    public class RouteLogIntegrationService : IRouteLogIntegrationService
    {
        /// <summary>
        /// Delivery route web API name
        /// </summary>
        private string deliveryRouteWebAPIName = string.Empty;



        /// <summary>
        /// PDF generator web API name
        /// </summary>
        private string pdfGeneratorWebAPIName = string.Empty;



        /// <summary>
        /// Reference to the HTTP handler
        /// </summary>
        private IHttpHandler httpHandler = default(IHttpHandler);



        /// <summary>
        /// Reference to the logging helper
        /// </summary>
        private ILoggingHelper loggingHelper = default(ILoggingHelper);





        /// <summary>
        /// Initializes a new instance of the <see cref="RouteLogIntegrationService" /> class
        /// </summary>
        /// <param name="httpHandler">HTTP handler</param>
        /// <param name="configurationHelper">Configuration helper</param>
        /// <param name="loggingHelper">Logging helper</param>
        public RouteLogIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (httpHandler == null) { throw new ArgumentNullException(nameof(httpHandler)); }
            if (configurationHelper == null) { throw new ArgumentNullException(nameof(configurationHelper)); }
            if (loggingHelper == null) { throw new ArgumentNullException(nameof(loggingHelper)); }


            // Store the injected dependencies
            this.httpHandler = httpHandler;
            this.loggingHelper = loggingHelper;


            // Retrieve the configuration settings
            //
            // Delivery Route Manager Web API name
            const string DeliveryRouteManagerWebAPINameConfigurationKey = "DeliveryRouteManagerWebAPIName";
            this.deliveryRouteWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryRouteManagerWebAPINameConfigurationKey).ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(this.deliveryRouteWebAPIName)) { throw new System.Exception($"Configuration setting {DeliveryRouteManagerWebAPINameConfigurationKey} must not be null or empty."); } // TODO update exception type

            // PDF Generator Web API name
            const string PDFGeneratorWebAPINameConfigurationKey = "PDFGeneratorWebAPIName";
            this.pdfGeneratorWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PDFGeneratorWebAPINameConfigurationKey).ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(this.pdfGeneratorWebAPIName)) { throw new System.Exception($"Configuration setting {PDFGeneratorWebAPINameConfigurationKey} must not be null or empty."); } // TODO update exception type
        }





        /// <summary>
        /// Retrieves the route log the specified delivery route
        /// </summary>
        /// <param name="deliveryRoute">The delivery route</param>
        /// <returns>The route log for the specified delivery route</returns>
        public async Task<RouteLogSummaryDTO> GetRouteLog(RouteDTO deliveryRoute)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Integration.{nameof(GetRouteLog)}"))
            {
                string methodName = typeof(RouteLogIntegrationService) + "." + nameof(GetRouteLog);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogIntegrationServiceMethodEntryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryRouteWebAPIName + "deliveryroute/deliveryroutesummaries/", deliveryRoute);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                // Get the route log from the result
                var routeLog = JsonConvert.DeserializeObject<RouteLogSummaryDTO>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogIntegrationServiceMethodExitEventId);
                return routeLog;
            }
        }



        /// <summary>
        /// Creates a PDF document file from an XML report document expressed as a string using the default
        ///   XSLFO template (FMO_PDFReport_Generic.xslt) and returns the name of the PDF document file
        /// The XML report document must be compliant with FMO_PDFReport_Generic.xsd
        /// </summary>
        /// <param name="reportXml">XML report document that is compliant with FMO_PDFReport_Generic.xsd</param>
        /// <returns>The PDF document file name</returns>
        public async Task<string> GeneratePdfDocument(string reportXml)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Integration.{nameof(GeneratePdfDocument)}"))
            {
                string methodName = typeof(RouteLogIntegrationService) + "." + nameof(GeneratePdfDocument);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogIntegrationServiceMethodEntryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(pdfGeneratorWebAPIName + "PDFReports", reportXml);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                // Get the PDF document file name from the result
                var pdfFileName = result.Content.ReadAsStringAsync().Result;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogIntegrationServiceMethodExitEventId);
                return pdfFileName;
            }
        }
    }
}