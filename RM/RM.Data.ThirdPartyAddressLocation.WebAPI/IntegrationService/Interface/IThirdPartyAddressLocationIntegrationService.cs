using System;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService
{
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
    }
}