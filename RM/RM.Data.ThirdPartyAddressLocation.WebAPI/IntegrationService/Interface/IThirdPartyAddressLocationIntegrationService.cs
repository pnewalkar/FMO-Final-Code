using System;
using System.Threading.Tasks;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using System.Collections.Generic;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService
{
    /// <summary>
    /// Interface definition for the Third Party integration Service members
    /// </summary>
    public interface IThirdPartyAddressLocationIntegrationService
    {
        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        Task<bool> DeliveryPointExists(int uDPRN);

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByUDPRNForThirdParty(int uDPRN);

        /// <summary>
        ///  Retreive GUID for specified category
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="strRefDataName">Reference data Name</param>
        /// <returns>GUID</returns>
        Task<Guid> GetReferenceDataId(string strCategoryname, string strRefDataName);

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDTO as DTO</param>
        /// <returns>updated delivery point</returns>
        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO);

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        Task<bool> CheckIfNotificationExists(int uDPRN, string action);

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action);

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> AddNewNotification(NotificationDTO notificationDTO);

        /// <summary>
        /// Get the PostCodeSectorDTO on UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO</returns>
        Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN);

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        Task<PostalAddressDTO> GetPostalAddress(int uDPRN);

        /// <summary>
        /// Get PAF address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        Task<PostalAddressDTO> GetPAFAddress(int uDPRN);

        /// <summary>
        /// Get Delivery Point details depending on the UDPRN
        /// </summary>
        /// <param name="addressId">Postal Address id</param>
        /// <returns>returns DeliveryPoint object</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId);

        /// <summary>
        /// Delete Delivery Point details depending on the DeliveryPoint id
        /// </summary>
        /// <param name="addressId">Delivery Point id</param>
        /// <returns>returns whether Delivery Point is deleted or not</returns>
        Task<bool> DeleteDeliveryPoint(Guid id);

        /// <summary>
        /// Insert new Delivery Point details depending on the DeliveryPoint id
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery Point object</param>
        /// <returns>returns whether Delivery Point is created or not</returns>
        Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        /// <summary>
        /// Get the reference data category details based on the list of categores
        /// </summary>
        /// <param name="listNames">Category names</param>
        /// <returns>returns Category details</returns>
        Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);

        /// <summary>
        /// Update the Notification By UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="oldAction">old action</param>
        /// <param name="newAction">new action</param>
        /// <returns></returns>
        Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction);
    }
}