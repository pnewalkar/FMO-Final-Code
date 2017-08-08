using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<bool> ProcessPAFDetails(List<PostalAddressDTO> postalAddress);

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="postalAddressId">PostalAddress Unique Identifier</param>
        /// <returns>Postal Address DTO</returns>
        PostalAddressDTO GetPostalAddressDetails(Guid postalAddressId);

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress as input</param>
        /// <returns>string</returns>
        Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// This method is used to Create Address and Delivery Point.
        /// </summary>
        /// <param name="addDeliveryPointDTO">AddDeliveryPointDTO as input</param>
        /// <returns>CreateDeliveryPointModelDTO</returns>
        CreateDeliveryPointModelDTO CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO);

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        Task<PostalAddressDTO> GetPostalAddress(int? uDPRN);

        /// <summary>
        ///  Get Postal Addresses on adress guid's  as search criteria
        /// </summary>
        /// <param name="addressGuids"></param>
        /// <returns></returns>
        Task<List<PostalAddressDTO>> GetPostalAddresses(List<Guid> addressGuids);

        /// <summary>
        /// Get Postal Address on UDPRN value
        /// </summary>
        /// <param name="udprn">udprn value of PostalAddress</param>
        /// <returns></returns>
        Task<PostalAddressDTO> GetPAFAddress(int udprn);

        /// <summary>
        /// Delete Postal Addresses as part of Housekeeping
        /// </summary>
        /// <returns>Void</returns>
        Task DeletePostalAddressesForHouseKeeping();
    }
}