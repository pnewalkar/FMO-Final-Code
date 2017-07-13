﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;

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
        Task<bool> SaveAddress(PostalAddressDataDTO objPostalAddress, string fileName);

        /// <summary>
        /// Update PostalAddress based on the PostalAddressDataDTO passed for PAF depending upon NYB and USR scenerios
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDataDTO</param>
        /// <param name="fileName">Passing File Name in case PAF, NYB to track error</param>
        /// <param name="deliveryPointUseIndicatorPAF">Guid of DeliveryPointUseIndicatorPAF from reference data</param>
        /// <returns>return status as bool</returns>
        Task<bool> UpdateAddress(PostalAddressDataDTO objPostalAddress, string fileName, Guid deliveryPointUseIndicatorPAF);

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
        /// <param name="id">postCode</param>
        /// <returns>Postal Address DTO</returns>
        PostalAddressDataDTO GetPostalAddressDetails(Guid id);

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
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO, Guid OperationalStatus);

        /// <summary>
        /// Check For Duplicate Address With DeliveryPoints
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <returns>bool</returns>
        bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress);

        Task<List<PostalAddressDataDTO>> GetPostalAddresses(List<Guid> addressGuids);

        /*Task<List<Guid>> GetPostcodeGuids(string searchText);

        Task<List<Guid>> GetSelectedPostcode(string selectedItem);*/

        Task<PostalAddressDTO> GetPAFAddress(int udprn, Guid pafGuid);
    }
}