using RM.CommonLibrary.EntityFramework.DTO;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
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
        Task<NotificationDTO> GetNotificationByUDPRN(int uDPRN);

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
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        Task<bool> CheckIfNotificationExists(int uDPRN, string action);
    }
}