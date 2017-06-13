using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    public interface IRouteLogBusinessService
    {
        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRouteDto">deliveryRouteDto</param>
        /// <returns>deliveryRouteDto</returns>
        Task<string> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto);
    }
}