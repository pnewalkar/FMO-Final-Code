using RM.CommonLibrary.EntityFramework.DTO;
using System.Threading.Tasks;

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
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN);
    }
}