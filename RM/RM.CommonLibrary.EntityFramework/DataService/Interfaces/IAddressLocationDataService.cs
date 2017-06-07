using RM.CommonLibrary.EntityFramework.DTO;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    /// <summary>
    /// Address Location DataService interface to interact with the entities
    /// </summary>
    public interface IAddressLocationDataService
    {
        /// <summary>
        /// Get the Address location for the specified UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        Task<AddressLocationDTO> GetAddressLocationByUDPRN(int udprn);

        /// <summary>
        /// Save the address location to the database
        /// </summary>
        /// <param name="addressLocationDTO">AddressLocationDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> SaveNewAddressLocation(AddressLocationDTO addressLocationDTO);

        /// <summary>
        /// Check if the address location exists
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>Task<int></returns>
        Task<bool> AddressLocationExists(int udprn);
    }
}