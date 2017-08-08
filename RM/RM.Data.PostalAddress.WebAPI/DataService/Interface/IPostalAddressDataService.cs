using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO;

namespace RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces
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
        Task<bool> SaveAddress(PostalAddressDataDTO objPostalAddress, string fileName, Guid operationalStatusGUID);

        /// <summary>
        /// Update PostalAddress based on the PostalAddressDataDTO passed for PAF depending upon NYB and USR scenerios
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDataDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <param name="deliveryPointUseIndicatorPAF">Guid of DeliveryPointUseIndicatorPAF from reference data</param>
        /// , Guid deliveryPointUseIndicatorPAF removed this param as DP changes is moved to their resp api
        /// <returns>return status as bool</returns>
        Task<bool> UpdateAddress(PostalAddressDataDTO objPostalAddress, string fileName);

        /// <summary>
        /// Get the existing postal address details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostalAddress DTO</returns>
        Task<PostalAddressDataDTO> GetPostalAddress(int? uDPRN);

        /// <summary>
        /// Get the existing postal address details based on the address for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDataDTO for search operation</param>
        /// <returns>PostalAddress DTO</returns>
        Task<PostalAddressDataDTO> GetPostalAddress(PostalAddressDataDTO objPostalAddress);

        /// <summary>
        /// Insert PostalAddress based on the PostalAddressDataDTO passed for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDataDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        Task<bool> InsertAddress(PostalAddressDataDTO objPostalAddress, string fileName);

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        Task<bool> DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);

        /*
        To be moved to Unit manager
        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs, List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postCodeDTOs);*/

        /*
         To be moved to Unit manager
        /// <summary>
        /// Get Postal Address based on postcode
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        Task<List<PostalAddressDataDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid, List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postcodeDTOs);*/

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="postalAddressId">PostalAddress Unique Identifier</param>
        /// <returns>Postal Address DTO</returns>
        PostalAddressDataDTO GetPostalAddressDetails(Guid postalAddressId);

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <param name="addressTypeNYBGuid">Reference data Guid of NYB</param>
        /// <returns>string</returns>
        string CheckForDuplicateNybRecords(PostalAddressDataDTO objPostalAddress, Guid addressTypeNYBGuid);

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="objPostalAddress">Postal address data DTO.</param>
        /// <returns>Guid</returns>
        Guid CreateAddressForDeliveryPoint(PostalAddressDataDTO objPostalAddress);

        /// <summary>
        /// Check For Duplicate Address With DeliveryPoints
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <returns>bool</returns>
        Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress);

        /// <summary>
        ///  Get Postal Addresses on adress guid's  as search criteria
        /// </summary>
        /// <param name="addressGuids">List of addressGuid</param>
        /// <returns></returns>
        Task<List<PostalAddressDataDTO>> GetPostalAddresses(List<Guid> addressGuids);

        /*Task<List<Guid>> GetPostcodeGuids(string searchText);

        Task<List<Guid>> GetSelectedPostcode(string selectedItem);*/

        /// <summary>
        /// Get Postal Address on UDPRN value
        /// </summary>
        /// <param name="udprn">udprn value of PostalAddress</param>
        /// <param name="pafGuid">pafGuid as Address Type Guid</param>
        /// <returns></returns>
        Task<PostalAddressDTO> GetPAFAddress(int udprn, Guid pafGuid);

        /// <summary>
        /// Delete postal Address details
        /// </summary>
        /// <param name="addressId">Postal Address Id</param>
        /// <returns>boolean</returns>
        Task<bool> DeletePostalAddress(Guid addressId);

        /// <summary>
        /// Update postal address status to live or pending delete
        /// </summary>
        /// <param name="postalAddressId">Address id</param>
        /// <param name="postalAddressStatus">Address status</param>
        /// <returns>boolean value true if status has been updated successfully</returns>
        Task<bool> UpdatePostalAddressStatus(Guid postalAddressId, Guid postalAddressStatus);

        /// <summary>
        /// Get All the pending delete postal addresses for deletion
        /// </summary>
        /// <param name="postalAddressPendingDeleteId">Postal Address Pending Delete Guid</param>
        /// <returns>Postal Adddress Data DTOs</returns>
        Task<List<PostalAddressDataDTO>> GetAllPendingDeletePostalAddresses(Guid postalAddressPendingDeleteId);

        /// <summary>
        /// Delete postal Addresses for housekeeping
        /// </summary>
        /// <param name="addressId">Postal Addresses Data DTOs</param>
        /// <returns>whether the records are delted or not</returns>
        Task<bool> DeletePostalAddressForHousekeeping(List<PostalAddressDataDTO> postalAddressDataDTOs);
    }
}