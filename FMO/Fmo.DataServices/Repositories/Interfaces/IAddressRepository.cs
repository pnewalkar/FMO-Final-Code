namespace Fmo.DataServices.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fmo.DTO;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to interact with postal address entity
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="fileName">CSV Filename</param>
        /// <returns>true or false</returns>
        bool SaveAddress(PostalAddressDTO objPostalAddress, string fileName);

        /// <summary>
        /// Update PostalAddress based on the PostalAddressDTO passed for PAF depending upon NYB and USR scenerios
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <param name="pafUpdateType">Passing 'USR' and 'NYB' to update</param>
        /// <returns>return status as bool</returns>
        bool UpdateAddress(PostalAddressDTO objPostalAddress, string fileName, string pafUpdateType);

        /// <summary>
        /// Get the existing postal address details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostalAddress DTO</returns>
        PostalAddressDTO GetPostalAddress(int? uDPRN);

        /// <summary>
        /// Get the existing postal address details based on the address for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO for search operation</param>
        /// <returns>PostalAddress DTO</returns>
        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// Insert PostalAddress based on the PostalAddressDTO passed for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        bool InsertAddress(PostalAddressDTO objPostalAddress, string fileName);

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        Task<List<PostalAddressDTO>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid);

        /// <summary>
        /// Get Postal Address based on postcode
        /// </summary>
        /// <param name="postCode">postCode</param>
        /// <param name="thoroughfare">thoroughfare</param>
        /// <returns>List of Postal Address</returns>
        Task<List<PostalAddressDTO>> GetPostalAddressDetails(string postCode);
    }
}