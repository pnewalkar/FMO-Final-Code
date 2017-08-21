using System.Threading.Tasks;
using RM.Operational.RouteLog.WebAPI.DTO;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Route Log Business Service interface definition
    /// </summary>
    public interface IRouteLogBusinessService
    {
        /// <summary>
        /// Generates a route log summary report for the specified delivery route and returns the file name
        ///   of the generated PDF document
        /// </summary>
        /// <param name="deliveryRoute">The delivery route</param>
        /// <returns>The PDF document file name</returns>
        Task<string> GenerateRouteLog(RouteDTO deliveryRoute);
    }
}