using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Extensions.FileProviders;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    public class PDFGeneratorBusinessService : IPDFGeneratorBusinessService
    {
        private string xsltFilepath = string.Empty;
        private IFileProvider fileProvider = default(IFileProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public PDFGeneratorBusinessService(IFileProvider fileProvider, IConfigurationHelper configurationHelper)
        {
            this.fileProvider = fileProvider;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.XSLTFilePath).ToString() : string.Empty;
        }

        public string GenerateRouteLogSummaryReport(string xml, string fileName)
        {
            IDirectoryContents contents = fileProvider.GetDirectoryContents(string.Empty);
            IFileInfo fileInfo = fileProvider.GetFileInfo(xsltFilepath + fileName);
            using (StreamReader reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                string xsltFilePath = reader.ReadToEnd();
                var xslTransformer = new XslCompiledTransform();
                XmlReader xsltReader = XmlReader.Create(new StringReader(xsltFilePath));
                XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
                StringWriter strWriter = new StringWriter();
                xslTransformer.Load(xsltReader);
                xslTransformer.Transform(xmlReader, null, strWriter);
                return strWriter.ToString();
            }
        }
    }
}