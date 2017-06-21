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
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.XSLTFilePath).ToString() : string.Empty;
            this.imagePath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.ImagePath).ToString() : string.Empty;
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
                printMapDTO.PrintTime = string.Format(Constants.PrintMapDateTimeFormat, DateTime.Now);
                SaveMapImage(printMapDTO);
            }

            return printMapDTO;
        }

        public async Task<string> GenerateMapPdfReport(PrintMapDTO printMapDTO)
        {
            string pdfFilename = string.Empty;
            if (printMapDTO != null)
            {
                string pdfXml = GenerateXml(printMapDTO);
                pdfFilename = await mapIntegrationService.GenerateReportWithMap(pdfXml, xsltFilepath);
            }

            return pdfFilename;
        }

        /// <summary>
        /// Custom serialization of Map DTO
        /// </summary>
        /// <param name="printMapDTO"></param>
        /// <returns></returns>
        private string GenerateXml(PrintMapDTO printMapDTO)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement report = doc.CreateElement("report");
            XmlElement pageHeader = doc.CreateElement("pageHeader");
            XmlElement pageFooter = doc.CreateElement("pageFooter");
            XmlElement content = doc.CreateElement("content");
            XmlElement heading1 = doc.CreateElement("heading1");
            XmlElement heading1CenterAligned = doc.CreateElement("heading1CenterAligned");
            XmlElement image = doc.CreateElement("image");
            XmlElement section = null;
            XmlElement sectionColumn = null;

            report.SetAttribute("outputTo", printMapDTO.PdfSize + printMapDTO.PdfOrientation);
            pageHeader.SetAttribute("caption", "");
            pageFooter.SetAttribute("caption", "");
            pageFooter.SetAttribute("pageNumbers", "true");
            report.AppendChild(pageHeader);
            report.AppendChild(pageFooter);
            report.AppendChild(content);

            //Section 1 Header
            section = doc.CreateElement("section");

            //Section 1 Header 1
            sectionColumn = doc.CreateElement("sectionColumn");
            sectionColumn.SetAttribute("width", "1");
            heading1CenterAligned.InnerText = printMapDTO.MapTitle;
            sectionColumn.AppendChild(heading1CenterAligned);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 2
            section = doc.CreateElement("section");

            //Section 2 columns 1 i.e Table 1
            sectionColumn = doc.CreateElement("sectionColumn");
            sectionColumn.SetAttribute("width", "1");
            image.SetAttribute("source", printMapDTO.ImagePath);
            sectionColumn.AppendChild(image);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);


            //Section 3
            section = doc.CreateElement("section");
            sectionColumn = doc.CreateElement("sectionColumn");
            sectionColumn.SetAttribute("width", "1");
            sectionColumn.InnerText = "Date : " + printMapDTO.PrintTime;
            section.AppendChild(sectionColumn);

            sectionColumn = doc.CreateElement("sectionColumn");
            sectionColumn.SetAttribute("width", "1");
            sectionColumn.InnerText = "Scale : " + printMapDTO.CurrentScale;
            section.AppendChild(sectionColumn);
            content.AppendChild(section);


            //Section 4
            section = doc.CreateElement("section");
            sectionColumn = doc.CreateElement("sectionColumn");
            sectionColumn.SetAttribute("width", "1");
            sectionColumn.InnerText = "TO DO *** Implement Licensing ***";
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            doc.AppendChild(report); ;
            return doc.InnerXml;
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