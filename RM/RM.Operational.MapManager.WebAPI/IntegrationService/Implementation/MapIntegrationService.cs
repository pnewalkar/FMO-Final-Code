using System.Net.Http;
using System.Threading.Tasks;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;

namespace RM.Operational.MapManager.WebAPI.IntegrationService
{
    public class MapIntegrationService : IMapIntegrationService
    {
        private const string PDFGeneratorWebAPIName = "PDFGeneratorWebAPIName";

        #region Property Declarations

        private string pdfGeneratorWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);

        #endregion Property Declarations

        #region Constructor

        public MapIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.pdfGeneratorWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PDFGeneratorWebAPIName).ToString() : string.Empty;
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
            pdfGeneratorWebAPIName = pdfGeneratorWebAPIName + "PDFReports/" + fileName;
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(pdfGeneratorWebAPIName, xml);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            return result.Content.ReadAsStringAsync().Result;
        }
    }
}