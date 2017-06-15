using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.DataManagement.PostalAddress.WebAPI.DTO;

namespace RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface
{
    /// <summary>
    /// Interface for Integration service
    /// </summary>
    public interface IPostalAddressIntegrationService
    {
        /// <summary>
        ///  Retreive reference data details from ReferenceData Web API
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        Task<Guid> GetReferenceDataGuId(string categoryName, string itemName);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listNames);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);

        /// <summary>
        /// Get the delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId);

        /// <summary>
        /// Insert the DeliveryPointDTO object into the database
        /// </summary>
        /// <param name="objDeliveryPoint">DeliveryPointDTO object</param>
        /// <returns>boolean value</returns>
        Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> AddNewNotification(NotificationDTO notificationDTO);

        /// <summary>
        /// Get the Address location for the specified UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        Task<AddressLocationDTO> GetAddressLocationByUDPRN(int udprn);

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Task<Guid> GetPostCodeID(string postCode);
    }
}