using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.Operational.MapManager.WebAPI.IntegrationService;
using Fonet;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    public class MapBusinessService : IMapBusinessService
    {
        private string xsltFilepath = string.Empty;
        private IMapIntegrationService mapIntegrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public MapBusinessService(IMapIntegrationService mapIntegrationService, IConfigurationHelper configurationHelper)
        {
            this.mapIntegrationService = mapIntegrationService;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.XSLTFilePath).ToString() : string.Empty;
        }

        /// <summary>
        /// Method to retrieve map details
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>deliveryRouteDto</returns>
        public async Task<byte[]> GenerateReportWithMap(PrintMapDTO printMapDTO)
        {
            string pdXslFo = string.Empty;
            var routeLogSummaryModelDTO = await mapIntegrationService.GenerateReportWithMap("", "");

            return GeneratePdf(pdXslFo);
        }

        private byte[] GeneratePdf(string xml)
        {
            MemoryStream stream = new MemoryStream();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            FonetDriver driver = FonetDriver.Make();
            driver.Render(xmlDocument, stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Serialze object to xml
        /// </summary>
        /// <typeparam name="T">Class </typeparam>
        /// <param name="type">object</param>
        /// <returns>xml as string</returns>
        private string XmlSerializer<T>(T type)
        {
            var xmlSerilzer = new XmlSerializer(type.GetType());
            var xmlString = new StringWriter();
            xmlSerilzer.Serialize(xmlString, type);
            return xmlString.ToString();
        }
    }
}