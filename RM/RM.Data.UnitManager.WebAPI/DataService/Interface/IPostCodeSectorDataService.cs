using System;
using System.Threading.Tasks;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// IPostcodeSectorDataService interface to abstract away the PostCodeSectorDataService implementation
    /// </summary>
    public interface IPostcodeSectorDataService
    {
        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="postcodeSectorTypeGuid">Postcode Sector Type Guid</param>
        /// <param name="postcodeDistrictTypeGuid">Postcode District Type Guid</param>
        /// <returns>PostCodeSectorDataDTO</returns>
        Task<PostcodeSectorDataDTO> GetPostcodeSectorByUdprn(int udprn, Guid postcodeSectorTypeGuid, Guid postcodeDistrictTypeGuid);
    }
}