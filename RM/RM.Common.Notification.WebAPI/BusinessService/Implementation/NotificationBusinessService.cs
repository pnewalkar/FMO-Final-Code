using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Common.Notification.WebAPI.BusinessService
{
    public class NotificationBusinessService : INotificationBusinessService
    {
        private INotificationDataService notificationDataService = default(INotificationDataService);

        public NotificationBusinessService(INotificationDataService notificationDataService)
        {
            this.notificationDataService = notificationDataService;
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            return await notificationDataService.AddNewNotification(notificationDTO);
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            return await notificationDataService.CheckIfNotificationExists(uDPRN, action);
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            return await notificationDataService.DeleteNotificationbyUDPRNAndAction(uDPRN, action);
        }

        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        public async Task<NotificationDTO> GetNotificationByUDPRN(int uDPRN)
        {
            return await notificationDataService.GetNotificationByUDPRN(uDPRN);
        }

        public async Task<bool> UpdateNotificationByUDPRN(int uDPRN, string oldAction, string newAction)
        {
            return await notificationDataService.UpdateNotificationByUDPRN(uDPRN, oldAction, newAction);
        }

        public async Task<bool> UpdateNotificationMessageByUDPRN(int uDPRN, string action, string message)
        {
            return await notificationDataService.UpdateNotificationMessageByUDPRN(uDPRN, action, message);
        }
    }
}