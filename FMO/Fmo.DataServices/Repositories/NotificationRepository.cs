using System;
using System.Linq;
using System.Threading.Tasks;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// To interact with the Notification database entity.
    /// </summary>
    public class NotificationRepository : RepositoryBase<Notification, FMODBContext>, INotificationRepository
    {
        public NotificationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            Notification newNotification = new Notification();
            GenericMapper.Map(notificationDTO, newNotification);
            DataContext.Notifications.Add(newNotification);
            return await DataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.Notification_Id == uDPRN && notific.Notification_Heading.Trim().Equals(action)).SingleOrDefault();
                DataContext.Notifications.Remove(notification);
                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        public NotificationDTO GetNotificationByUDPRN(int uDPRN)
        {
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.Notification_Id == uDPRN).SingleOrDefault();
                NotificationDTO notificationDTO = new NotificationDTO();
                GenericMapper.Map(notification, notificationDTO);
                return notificationDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public bool CheckIfNotificationExists(int uDPRN, string action)
        {
            try
            {
                if (DataContext.Notifications.AsNoTracking().Where(notific => notific.Notification_Id == uDPRN && notific.Notification_Heading.Trim().Equals(action)).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}