using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.Operational.MapManager.WebAPI.IntegrationService;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    public class MapBusinessService : IMapBusinessService
    {
        private const string XSLTFilePath = "XSLTFilePath";
        private const string ImagePath = "ImagePath";
        private const string PrintMapDateTimeFormat = "{0:dd/MM/yyyy HH:mm}";

        private string xsltFilepath = string.Empty;
        private string imagePath = string.Empty;
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
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(XSLTFilePath).ToString() : string.Empty;
            this.imagePath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ImagePath).ToString() : string.Empty;
        }

        /// <summary>
        /// Method to retrieve map details
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>deliveryRouteDto</returns>
        public PrintMapDTO SaveImage(PrintMapDTO printMapDTO)
        {
            string pdXslFo = string.Empty;
            if (printMapDTO != null)
            {
                printMapDTO.PrintTime = string.Format(PrintMapDateTimeFormat, DateTime.Now);
                SaveMapImage(printMapDTO);
            }

            return printMapDTO;
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

        private void SaveMapImage(PrintMapDTO printMapDTO)
        {
            string[] encodedStringArray = printMapDTO.EncodedString.Split(',');
            string imageLocation = imagePath + Guid.NewGuid() + ".png";

            if (encodedStringArray != null && encodedStringArray.Count() > 0)
            {
                byte[] imageBytes = Convert.FromBase64String(encodedStringArray[1]);
                File.WriteAllBytes(imageLocation, imageBytes);
            }

            printMapDTO.ImagePath = imageLocation;
        }
    }
}