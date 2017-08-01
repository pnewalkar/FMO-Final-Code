using Fonet;
using Fonet.Render.Pdf;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Xsl;

[assembly: InternalsVisibleTo("RM.CommonLibrary.Reporting.Pdf.Test")]
namespace RM.CommonLibrary.Reporting.Pdf
{
    /// <summary>
    /// PDF Generator class
    /// 
    /// Contains methods to create PDF documents using the Fonet (FO.NET) XSLFO processor
    /// 
    /// Use one of the overloads of the CreatePDF method to create a PDF at a specified file location using
    ///   the specified report XML (an XML file containing report data that is transformed using either the
    ///   default XSLT file or a specified XSLT file. Use the default XSLT file if the report XML is created
    ///   using the methods in the ReportFactoryHelper class. Use a specified XSLT file in all other cases -
    ///   it is the responsibility of the caller to ensure that the XSLT file is valid and generates valid
    ///   XSLFO output from the report XML.
    /// This class, the associated ReportFactoryHelper class and the default XSLT file have been designed to
    ///   work with Fonet (FO.NET) XSLFO processor. Different processors implement different subsets of the
    ///   FO standard so if another XSLFO processor is used it may be necessary to update the XSLT file to
    ///   produce output that renders correctly.
    /// </summary>
    public static class PdfGenerator
    {
        /// <summary>
        /// Gets the path to the default XSLT file
        /// </summary>
        private static string DefaultXsltFilePath
        {
            get
            {
                // Construct the path to the default XSLT file
                const string defaultXsltFolderName = "Xslt";
                const string defaultXsltFileName = "FMO_PDFReport_Generic.xslt";
                string xsltFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultXsltFolderName, defaultXsltFileName);


                // Return the path if the default XSLT file exists, otherwise throw an exception
                if (File.Exists(xsltFilePath))
                {
                    return xsltFilePath;
                }
                else
                {
                    // Throw an exception but do not include the path in the error message as this is
                    //   sensitive information
                    throw new FileNotFoundException($"The default XSLT file does not exist."); 
                }
            }
        }



        /// <summary>
        /// Creates a PDF from an XML representation of a report using the default XSLT file
        /// The default XSLT file is designed to be used with reports that are generated using the
        ///   ReportFactoryHelper class
        /// </summary>
        /// <param name="reportXml">String containing the XML representation of the report</param>
        /// <param name="pdfFilePath">The full path to the output PDF file (existing file will be overwritten)</param>
        /// <returns>True if the PDF document was created successfully, otherwise false</returns>
        public static bool CreatePDF(string reportXml, string pdfFilePath)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(reportXml)) { throw new ArgumentException($"{nameof(reportXml)} must not be null or empty."); }
            if (string.IsNullOrWhiteSpace(pdfFilePath)) { throw new ArgumentException($"{nameof(pdfFilePath)} must not be null or empty."); } 


            // Use the default XSLT file path
            string xsltFilePath = DefaultXsltFilePath;

            // Create the PDF
            return CreatePDF(reportXml, pdfFilePath, xsltFilePath);
        }



        /// <summary>
        /// Creates a PDF from an XML representation of a report using the specified XSLT file
        /// </summary>
        /// <param name="reportXml">String containing the XML representation of the report</param>
        /// <param name="pdfFilePath">The full path to the output PDF file (existing file will be overwritten)</param>
        /// <param name="xsltFilePath">The full path to the XSLT file</param>
        /// <returns>True if the PDF document was created successfully, otherwise false</returns>
        public static bool CreatePDF(string reportXml, string pdfFilePath, string xsltFilePath)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(reportXml)) { throw new ArgumentException($"{nameof(reportXml)} must not be null or empty."); } 
            if (string.IsNullOrWhiteSpace(pdfFilePath)) { throw new ArgumentException($"{nameof(pdfFilePath)} must not be null or empty."); } 
            if (string.IsNullOrWhiteSpace(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} must not be null or empty."); } 
            if (!File.Exists(xsltFilePath)) { throw new FileNotFoundException($"{nameof(xsltFilePath)} must point to a valid file."); }


            // Load the report XML into an XML document
            XmlDocument reportDocument = new XmlDocument();
            reportDocument.LoadXml(reportXml);

            // Create the PDF
            return CreatePDF(reportDocument, pdfFilePath, xsltFilePath);
        }



        /// <summary>
        /// Creates a PDF from an XML representation of a report using the default XSLT file
        /// The default XSLT file is designed to be used with reports that are generated using the
        ///   ReportFactoryHelper class
        /// </summary>
        /// <param name="reportDocument">XML document containing the report</param>
        /// <param name="pdfFilePath">The full path to the output PDF file (existing file will be overwritten)</param>
        /// <returns>True if the PDF document was created successfully, otherwise false</returns>
        public static bool CreatePDF(XmlDocument reportDocument, string pdfFilePath)
        {
            // Validate the arguments
            if (reportDocument == null) { throw new ArgumentNullException(nameof(reportDocument)); }
            if (string.IsNullOrWhiteSpace(pdfFilePath)) { throw new ArgumentException($"{nameof(pdfFilePath)} must not be null or empty."); } 


            // Use the default XSLT file path
            string xsltFilePath = DefaultXsltFilePath;

            // Create the PDF
            return CreatePDF(reportDocument, pdfFilePath, xsltFilePath);
        }



        /// <summary>
        /// Creates a PDF from an XML representation of a report using the specified XSLT file
        /// </summary>
        /// <param name="reportDocument">XML document containing the report</param>
        /// <param name="pdfFilePath">The full path to the output PDF file (existing file will be overwritten)</param>
        /// <param name="xsltFilePath">The full path to the XSLT file</param>
        /// <returns>True if the PDF document was created successfully, otherwise false</returns>
        public static bool CreatePDF(XmlDocument reportDocument, string pdfFilePath, string xsltFilePath)
        {
            // Validate the arguments
            if (reportDocument == null) { throw new ArgumentNullException(nameof(reportDocument)); } 
            if (string.IsNullOrWhiteSpace(pdfFilePath)) { throw new ArgumentException($"{nameof(pdfFilePath)} must not be null or empty."); } 
            if (string.IsNullOrWhiteSpace(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} must not be null or empty."); }
            if (!File.Exists(xsltFilePath)) { throw new FileNotFoundException($"{nameof(xsltFilePath)} must point to a valid file."); }


            // Assume that the PDF cannot be created
            bool success = false;

            // Get the report XSLFO (the report document transformed using the xslt)
            string reportXslFo = GetReportXslFo(reportDocument, xsltFilePath);
            if (!string.IsNullOrWhiteSpace(reportXslFo))
            {
                // Create the PDF from the report XSLFO
                success = CreatePdfFromXslFo(reportXslFo, pdfFilePath);
            }

            return success;
        }



        /// <summary>
        /// Creates a PDF from an XSLFO representation of a report
        /// </summary>
        /// <param name="reportXslFo">String containing the XSLFO representation of the report</param>
        /// <param name="pdfFilePath">The full path to the output PDF file (existing file will be overwritten)</param>
        /// <returns>True if the PDF document was created successfully, otherwise false</returns>
        private static bool CreatePdfFromXslFo(string reportXslFo, string pdfFilePath)
        {
            // Validate the arguments
            if (string.IsNullOrWhiteSpace(reportXslFo)) { throw new ArgumentException($"{nameof(reportXslFo)} must not be null or empty."); } 
            if (string.IsNullOrWhiteSpace(pdfFilePath)) { throw new ArgumentException($"{nameof(pdfFilePath)} must not be null or empty."); } 


            // Load the report XSLFO document into an XML document
            XmlDocument xslfoDocument = new XmlDocument();
            xslfoDocument.LoadXml(reportXslFo);


            // Create a stream to write the PDF file into
            using (Stream pdfStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
            {
                // Set up the Fonet driver
                FonetDriver driver = FonetDriver.Make();

                // Configure the font options
                PdfRendererOptions options = new PdfRendererOptions();
                options.FontType = FontType.Link; // TODO or FontType.Embed or FontType.Subset
                                                  // options.AddPrivateFont(new FileInfo(@"\\server\fonts\rmgfont.otf"));
                driver.Options = options;

                // Create the PDF from the XSL-FO document
                driver.Render(xslfoDocument, pdfStream);
            }


            // Determine whether the PDF creation was successful
            bool success = File.Exists(pdfFilePath);
            return success;
        }



        /// <summary>
        /// Creates an XSLFO representation of an XML report document using the specified XSLT file
        /// </summary>
        /// <param name="reportDocument">XML document containing the report</param>
        /// <param name="xsltFilePath">The full path to the XSLT file</param>
        /// <returns>A string containing the XSLFO representation of the report</returns>
        private static string GetReportXslFo(XmlDocument reportDocument, string xsltFilePath)
        {
            // Validate the arguments
            if (reportDocument == null) { throw new ArgumentNullException(nameof(reportDocument)); }
            if (string.IsNullOrWhiteSpace(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} must not be null or empty."); } 
            if (!File.Exists(xsltFilePath)) { throw new ArgumentException($"{nameof(xsltFilePath)} does not exist."); } 


            // Load the XSLT into an XSL compiled transform
            var transform = new XslCompiledTransform();
            transform.Load(xsltFilePath);

            // Transform the report XML using the XSLT to get the report XSLFO
            string reportXslFo = null;
            using (StringWriter sw = new StringWriter())
            {
                transform.Transform(reportDocument, null, sw);
                reportXslFo = sw.ToString();
            }

            // Return the report XSLFO
            return reportXslFo;
        }
    }
}
