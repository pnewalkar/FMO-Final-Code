namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class NotificationRepository : RepositoryBase<Notification, FMODBContext>, INotificationRepository
    {
        public NotificationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            try
            {
                Notification newNotification = new Notification();
                GenericMapper.Map(notificationDTO, newNotification);
                DataContext.Notifications.Add(newNotification);
                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.Notification_Id.Equals(uDPRN) && notific.Notification_Heading.Trim().Equals(action)).SingleOrDefault();
                DataContext.Notifications.Remove(notification);
                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

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
    }
}
