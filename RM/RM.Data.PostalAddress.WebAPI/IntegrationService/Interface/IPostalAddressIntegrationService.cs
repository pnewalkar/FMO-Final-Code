using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
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
        /// This method will call Delivery point web api which is used to
        /// update delivery point for resp PostalAddress which has type <USR>.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        Task<bool> UpdateDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

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

        Task<List<PostcodeDTO>> GetPostcodes(Guid unitGuid, List<Guid> postcodeGuids);

        Task<PostcodeDTO> GetSelecetdPostcode(Guid postcodeGuid, Guid unitGuid);

        Task<bool> CheckIfNotificationExists(int uDPRN, string action);

        Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction);

        Task<bool> UpdateNotificationMessageByUDPRN(int udprn, string action, string message);

        // <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns>The approx location/</returns>
        Task<DbGeometry> GetApproxLocation(string postcode);

        Task<bool> UpdateDPUse(PostalAddressDTO postalAddressDetails);
    }
}