using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.PDFGenerator.WebAPI.BusinessService
{
    public interface IPDFGeneratorBusinessService
    {
        string GenerateRouteLogSummaryReport(string xml, string fileName);

        PdfFileDTO GeneratePdfReport(string pdfFilename);
    }
}