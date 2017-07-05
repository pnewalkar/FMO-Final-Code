using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;

namespace RM.Operational.RouteLog.WebAPI.IntegrationService
{
    public interface IRouteLogIntegrationService
    {
        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRouteDto">deliveryRouteDto</param>
        /// <returns>deliveryRouteDto</returns>
        Task<RouteLogSummaryModelDTO> GenerateRouteLog(RouteDTO deliveryRouteDto);

        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns></returns>
        Task<string> GenerateRouteLogSummaryReport(string xml, string fileName);
    }
}