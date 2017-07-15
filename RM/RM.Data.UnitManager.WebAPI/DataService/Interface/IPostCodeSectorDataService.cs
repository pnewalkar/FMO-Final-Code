using System;
using System.Threading.Tasks;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// IPostCodeSectorDataService interface to abstract away the PostCodeSectorDataService implementation
    /// </summary>
    public interface IPostCodeSectorDataService
    {
        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="postCodeSectorTypeGuid">postCodeSectorType Guid</param>
        /// <param name="postCodeDistrictTypeGuid">postCodeDistrictType Guid</param>
        /// <returns>PostCodeSectorDataDTO</returns>
        Task<PostCodeSectorDataDTO> GetPostCodeSectorByUdprn(int udprn, Guid postCodeSectorTypeGuid, Guid postCodeDistrictTypeGuid);
    }
}