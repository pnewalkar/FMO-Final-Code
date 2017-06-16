using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    public class RouteLogIntegrationService : IRouteLogIntegrationService
    {
        #region Property Declarations

        private string deliveryRouteWebAPIName = string.Empty;
        private string pdfGeneratorWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);

        #endregion Property Declarations

        #region Constructor

        public RouteLogIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.deliveryRouteWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.pdfGeneratorWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.PDFGeneratorWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRouteDto">deliveryRouteDto</param>
        /// <returns>deliveryRouteDto</returns>
        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto)
        {
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryRouteWebAPIName + "deliveryroute/deliveryroutesummaries/", deliveryRouteDto);
            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            return JsonConvert.DeserializeObject<RouteLogSummaryModelDTO>(result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns>byte array</returns>
        public async Task<string> GenerateRouteLogSummaryReport(string xml, string fileName)
        {
            pdfGeneratorWebAPIName = pdfGeneratorWebAPIName + "PDFReports/" + fileName;
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(pdfGeneratorWebAPIName, xml);
            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            return result.Content.ReadAsStringAsync().Result;
        }
    }
}