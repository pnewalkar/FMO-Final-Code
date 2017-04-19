using Fmo.DTO;
using Fmo.Entities;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// Address Location Repository interface to interact with the entities
    /// </summary>
    public interface IAddressLocationRepository
    {
        /// <summary>
        /// Get the Address location for the specified UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        AddressLocationDTO GetAddressLocationByUDPRN(int uDPRN);

        /// <summary>
        /// Save the address location to the database
        /// </summary>
        /// <param name="addressLocationDTO">AddressLocationDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> SaveNewAddressLocation(AddressLocationDTO addressLocationDTO);

        /// <summary>
        /// Check if the address location exists
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>Task<int></returns>
        bool AddressLocationExists(int uDPRN);
    }
}