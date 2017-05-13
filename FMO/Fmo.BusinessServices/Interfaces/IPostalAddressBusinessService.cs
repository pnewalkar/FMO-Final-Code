﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.DTO.Model;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// Interface for USR Business service
    /// </summary>
    public interface IPostalAddressBusinessService
    {
        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>return success</returns>
        bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName);

        /// <summary>
        /// Save list of PAF details into database.
        /// </summary>
        /// <param name="postalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        bool SavePAFDetails(List<PostalAddressDTO> postalAddress);

        /// <summary>
        /// Method defination to save delivery point and Task for notification for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">Postal address DTO object</param>
        void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid);

        /// <summary>
        /// Get Postal Address based on postcode
        /// </summary>
        /// <param name="postCode">postCode</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        Task<PostalAddressDTO> GetPostalAddressDetails(string postCode, Guid unitGuid);

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        PostalAddressDTO GetPostalAddressDetails(Guid id);

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress as input</param>
        /// <returns>string</returns>
        string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// This method is used to Create Address and Delivery Point.
        /// </summary>
        /// <param name="addDeliveryPointDTO">AddDeliveryPointDTO as input</param>
        /// <returns>CreateDeliveryPointModelDTO</returns>
        CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO);
    }
}