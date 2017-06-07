using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    /// <summary>
    /// Interface to interact with postal address entity
    /// </summary>
    public interface IPostalAddressDataService
    {
        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="fileName">CSV Filename</param>
        /// <returns>true or false</returns>
        Task<bool> SaveAddress(PostalAddressDTO objPostalAddress, string fileName);

        /// <summary>
        /// Update PostalAddress based on the PostalAddressDTO passed for PAF depending upon NYB and USR scenerios
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <param name="deliveryPointUseIndicatorPAF">Guid of DeliveryPointUseIndicatorPAF from reference data</param>
        /// <returns>return status as bool</returns>
        Task<bool> UpdateAddress(PostalAddressDTO objPostalAddress, string fileName, Guid deliveryPointUseIndicatorPAF);

        /// <summary>
        /// Get the existing postal address details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostalAddress DTO</returns>
        Task<PostalAddressDTO> GetPostalAddress(int? uDPRN);

        /// <summary>
        /// Get the existing postal address details based on the address for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO for search operation</param>
        /// <returns>PostalAddress DTO</returns>
        Task<PostalAddressDTO> GetPostalAddress(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// Insert PostalAddress based on the PostalAddressDTO passed for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        Task<bool> InsertAddress(PostalAddressDTO objPostalAddress, string fileName);

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        Task<bool> DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs);

        /// <summary>
        /// Get Postal Address based on postcode
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        Task<List<PostalAddressDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid);

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">postCode</param>
        /// <returns>Postal Address DTO</returns>
        PostalAddressDTO GetPostalAddressDetails(Guid id);

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <param name="addressTypeNYBGuid">Reference data Guid of NYB</param>
        /// <returns>string</returns>
        string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress, Guid addressTypeNYBGuid);

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO);

        /// <summary>
        /// Check For Duplicate Address With DeliveryPoints
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <returns>bool</returns>
        bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress);
    }
}