using System.Threading.Tasks;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DataService
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
        Task<AddressLocationDataDTO> GetAddressLocationByUDPRN(int udprn);

        /// <summary>
        /// Save the address location to the database
        /// </summary>
        /// <param name="addressLocationDTO">AddressLocationDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> SaveNewAddressLocation(AddressLocationDataDTO addressLocationDTO);

        /// <summary>
        /// Check if the address location exists
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>Task<int></returns>
        Task<bool> AddressLocationExists(int udprn);

        /// <summary>
        /// Get the Postal Address based on UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>Postal Address record</returns>
        Task<PostalAddressDataDTO> GetPostalAddressData(int udprn);

        /// <summary>
        /// Check if there are any notification for the given UDPRN and action
        /// </summary>
        /// <param name="udprn">UDPRN ID</param>
        /// <param name="action">action message to be updated</param>
        /// <returns>whether the notification exists or not</returns>
        Task<bool> CheckIfNotificationExists(int udprn, string action);
    }
}