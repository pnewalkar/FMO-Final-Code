using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    /// <summary>
    /// PDF Generator Business Service interface definition
    /// </summary>
    public interface IPDFGeneratorBusinessService
    {
        /// <summary>
        /// Generates a PDF report from the specified report XML document using the default XSLT file and returns 
        ///   the file name of the generated PDF document
        /// The default XSLT file is FMO_PDFReport_Generic.xslt in RM.CommonLibrary.Reporting.Pdf
        /// The report XML document must be compliant with the schema defined in FMO_PDFReport_Generic.xsd in
        ///   RM.CommonLibrary.Reporting.Pdf. The RM.CommonLibrary.Reporting.Pdf.ReportFactoryHelper class
        ///   provides methods that create compliant elements and attributes, but the onus is on the developer
        ///   to verify that the report XML document is compliant
        /// </summary>
        /// <param name="reportXml">The XML report containing the report definition</param>
        /// <returns>The PDF document file name</returns>
        string CreatePdfReport(string reportXml);



        /// <summary>
        /// Generates a PDF report from the specified report XML document using the specified XSLT file and returns
        ///   the file name of the generated PDF document
        /// The specified XSLT file must generate valid XSLFO output when applied to the report XML document
        /// </summary>
        /// <param name="reportXml">The XML report containing the report definition</param>
        /// <param name="xsltFilePath">The full path to the XSLT file</param>
        /// <returns>The PDF document file name</returns>
        string CreatePdfReport(string reportXml, string xsltFilePath);



        /// <summary>
        /// Deletes the PDF report with the specified PDF document file name
        /// </summary>
        /// <param name="pdfFileName">The PDF document file name</param>
        void DeletePdfReport(string pdfFileName);



        /// <summary>
        /// Gets the PDF report for the specified PDF document file name
        /// </summary>
        /// <param name="pdfFileName">The PDF document file name</param>
        /// <returns>The PDF report encoded as a byte array in a DTO</returns>
        PdfFileDTO GetPdfReport(string pdfFileName);
    }
}