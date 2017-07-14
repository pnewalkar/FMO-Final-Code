using RM.Operational.RouteLog.WebAPI.DTO;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    public interface IRouteLogBusinessService
    {
        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRoute">deliveryRoute</param>
        /// <returns>deliveryRoute</returns>
        Task<string> GenerateRouteLog(RouteDTO deliveryRoute);
    }
}