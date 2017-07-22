using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO.FileProcessing;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService
{
    /// <summary>
    /// Interface definition for the Third Party business Service members
    /// </summary>
    public interface IThirdPartyAddressLocationBusinessService
    {
        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>
        /// Address Location DTO
        /// </returns>
        Task<object> GetAddressLocationByUDPRNJson(int uDPRN);

        // To be implemented in parallel
        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos"> List of Address Locations</param>
        /// <returns> Task </returns>
        Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos);

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        Task<AddressLocationDTO> GetAddressLocationByUDPRN(int udprn);
    }
}