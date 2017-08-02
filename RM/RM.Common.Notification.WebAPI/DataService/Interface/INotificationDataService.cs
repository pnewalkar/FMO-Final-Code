using System.Threading.Tasks;
using RM.Common.Notification.WebAPI.DTO;

namespace RM.Common.Notification.WebAPI.DataService.Interface
{
    /// <summary>
    /// INotificationDataService interface to abstract away the NotificationDataService implementation
    /// </summary>
    public interface INotificationDataService
    {
        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        Task<NotificationDataDTO> GetNotificationByUDPRN(int uDPRN);

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
        Task<int> AddNewNotification(NotificationDataDTO notificationDTO);

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        Task<bool> CheckIfNotificationExists(int uDPRN, string action);

        /// <summary>
        /// Update the notifications on UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="oldAction">old Action</param>
        /// <param name="newAction">updated action</param>
        /// <returns>whether notification was updated or not</returns>
        Task<bool> UpdateNotificationByUDPRN(int uDPRN, string oldAction, string newAction);

        /// <summary>
        /// Update notification message by UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">Action</param>
        /// <param name="message">message</param>
        /// <returns>whether notification was updated or not</returns>
        Task<bool> UpdateNotificationMessageByUDPRN(int uDPRN, string action, string message);
    }
}