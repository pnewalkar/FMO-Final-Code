using Fmo.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.DBContext;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification, FMODBContext>, INotificationRepository
    {
        public NotificationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<int> DeleteNotificationbyUDPRN(int uDPRN)
        {
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.Notification_Id == uDPRN).SingleOrDefault();
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
