using System.Threading.Tasks;
using RM.Operational.RouteLog.WebAPI.DTO;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    /// <summary>
    /// Route Log Integration Service interface definition
    /// </summary>
    public interface IRouteLogIntegrationService
    {
        /// <summary>
        /// Retrieves the route log the specified delivery route
        /// </summary>
        /// <param name="deliveryRoute">The delivery route</param>
        /// <returns>The route log for the specified delivery route</returns>
        Task<RouteLogSummaryDTO> GetRouteLog(RouteDTO deliveryRoute);



        /// <summary>
        /// Creates a PDF document file from an XML report document expressed as a string using the default
        ///   XSLFO template (FMO_PDFReport_Generic.xslt) and returns the name of the PDF document file
        /// The XML report document must be compliant with FMO_PDFReport_Generic.xsd
        /// </summary>
        /// <param name="reportXml">XML report document that is compliant with FMO_PDFReport_Generic.xsd</param>
        /// <returns>The PDF document file name</returns>
        Task<string> GeneratePdfDocument(string reportXml);
    }
}