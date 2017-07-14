using RM.Operational.RouteLog.WebAPI.DTO;
using RM.Operational.RouteLog.WebAPI.DTO.Model;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    public interface IRouteLogIntegrationService
    {
        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRoute">deliveryRoute</param>
        /// <returns>deliveryRoute</returns>
        Task<RouteLogSummaryDTO> GenerateRouteLog(RouteDTO deliveryRoute);

        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns></returns>
        Task<string> GenerateRouteLogSummaryReport(string xml, string fileName);
    }
}