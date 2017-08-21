using System;
using System.Threading.Tasks;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// IPostcodeSectorDataService interface to abstract away the PostcodeSectorDataService implementation
    /// </summary>
    public interface IPostcodeSectorDataService
    {
        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>PostcodeSectorDataDTO</returns>
        Task<PostcodeSectorDataDTO> GetPostcodeSectorByUdprn(int udprn);
    }
}