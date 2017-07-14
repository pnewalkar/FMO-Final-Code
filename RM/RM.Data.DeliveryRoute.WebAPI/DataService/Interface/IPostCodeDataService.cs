using System.Threading.Tasks;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public interface IPostCodeDataService
    {
        /// <summary>
        /// Get postcode details by passing postcode
        /// </summary>
        /// <param name="postCodeUnit">Postcode</param>
        /// <returns></returns>
        Task<PostCodeDataDTO> GetPostCode(string postCodeUnit);
    }
}