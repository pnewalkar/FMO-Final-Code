using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// using RM.CommonLibrary.EntityFramework.DTO;
// using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface
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
        Task<bool> SavePostalAddressForNYB(List<PostalAddressDTO> lstPostalAddress, string strFileName);

        /// <summary>
        /// Save list of PAF details into database.
        /// </summary>
        /// <param name="postalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        Task<bool> SavePAFDetails(List<PostalAddressBatchDTO> postalAddress);

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
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        Task<PostalAddressDTO> GetPostalAddressDetails(string selectedItem, Guid unitGuid);

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

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        Task<PostalAddressDTO> GetPostalAddress(int? uDPRN);
    }
}