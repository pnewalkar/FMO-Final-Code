using RM.CommonLibrary.Reporting.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    internal static class MapFactory
    {
        /// <summary>
        /// Gets the Map XML Document for the specified data
        /// </summary>
        /// <param name="caption">The caption to display at the top of the map</param>
        /// <param name="sourcePath">The full path to the map source file</param>
        /// <param name="timestamp">The timestamp to display below the map</param>
        /// <param name="scale">The scale to display below the map</param>
        /// <param name="legalNotices">The legal notices to display below the map</param>
        /// <param name="options">The report options</param>
        /// <returns>An XML Document containing the XML ready for transformation using FMO_PDFReport_Generic.xslt</returns>
        public static XmlDocument GetMap(string caption, string sourcePath, string timestamp, string scale, string[] legalNotices, MapConfiguration options)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(sourcePath)) { throw new ArgumentNullException(nameof(sourcePath)); }

            // Create the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Create the report element
            XmlElement reportNode = ReportFactoryHelper.AddReportElement( xmlDoc, options.OutputTo, false);

            // Create the content element
            XmlElement contentNode = ReportFactoryHelper.AddContentElement( reportNode,  xmlDoc);

            // The content element contains a section that contains the caption, map, timestamp, scale and legal notices
            ReportFactoryHelper.AddMapSection(contentNode,  xmlDoc, caption, sourcePath, timestamp, scale, legalNotices);

            // Return the XML document containing the report
            return xmlDoc;
        }
    }
}
