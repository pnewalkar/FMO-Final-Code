using System.Threading.Tasks;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public interface IPostcodeDataService
    {
        /// <summary>
        /// Get postcode details by passing postcode
        /// </summary>
        /// <param name="postcodeUnit">Postcode</param>
        /// <returns></returns>
        Task<PostcodeDataDTO> GetPostcode(string postcodeUnit);
    }
}