using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
            if (printMapDTO != null)
            {
                printMapDTO.PrintTime = string.Format(Constants.PrintMapDateTimeFormat, DateTime.Now);
                SaveMapImage(printMapDTO);
            }

            return printMapDTO;
        }

        /// <summary>
        /// generate Print Map pdf using xsl fo
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Pdf file name </returns>
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
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>Xml as string</returns>
        private string GenerateXml(PrintMapDTO printMapDTO)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement report = doc.CreateElement(Constants.Report);
            XmlElement pageHeader = doc.CreateElement(Constants.PageHeader);
            XmlElement pageFooter = doc.CreateElement(Constants.PageFooter);
            XmlElement content = doc.CreateElement(Constants.Content);
            XmlElement heading1 = doc.CreateElement(Constants.Heading1);
            XmlElement heading1CenterAligned = doc.CreateElement(Constants.Heading1CenterAligned);
            XmlElement image = doc.CreateElement(Constants.Image);
            XmlElement section = null;
            XmlElement sectionColumn = null;

            report.SetAttribute(Constants.PdfOutPut, printMapDTO.PdfSize + printMapDTO.PdfOrientation);
            pageHeader.SetAttribute(Constants.Caption, string.Empty);
            pageFooter.SetAttribute(Constants.Caption, "");
            pageFooter.SetAttribute(Constants.PageNumber, string.Empty);
            report.AppendChild(pageHeader);
            report.AppendChild(pageFooter);
            report.AppendChild(content);

            //Section 1 Header
            section = doc.CreateElement(Constants.Section);

            //Section 1 Header 1
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            heading1CenterAligned.InnerText = printMapDTO.MapTitle;
            sectionColumn.AppendChild(heading1CenterAligned);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 2
            section = doc.CreateElement(Constants.Section);

            //Section 2 columns 1 i.e Table 1
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            image.SetAttribute(Constants.Source, printMapDTO.ImagePath);
            sectionColumn.AppendChild(image);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 3
            section = doc.CreateElement(Constants.Section);
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            sectionColumn.InnerText = "Date : " + printMapDTO.PrintTime;
            section.AppendChild(sectionColumn);

            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            sectionColumn.InnerText = "Scale : " + printMapDTO.CurrentScale;
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 4
            section = doc.CreateElement(Constants.Section);
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            sectionColumn.InnerText = printMapDTO.License;
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            doc.AppendChild(report); ;
            return doc.InnerXml;
        }

        /// <summary>
        /// Method to save captured map image in png format.
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        private void SaveMapImage(PrintMapDTO printMapDTO)
        {
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
        }
    }
}