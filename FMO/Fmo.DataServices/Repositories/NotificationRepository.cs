﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.Common.ExceptionManagement;
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
            try
            {
                Notification newNotification = new Notification();
                GenericMapper.Map(notificationDTO, newNotification);
                DataContext.Notifications.Add(newNotification);
                return await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new SqlException(dbUpdateException, string.Format(ErrorMessageConstants.SqlAddExceptionMessage, string.Concat("notification for:", notificationDTO.NotificationActionLink)));
            }
            catch (NotSupportedException notSupportedException)
            {
                throw new InfrastructureException(notSupportedException, ErrorMessageConstants.NotSupportedExceptionMessage);
            }
            catch (ObjectDisposedException disposedException)
            {
                throw new ServiceException(disposedException, ErrorMessageConstants.ObjectDisposedExceptionMessage);
            }
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            string actionLink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN);
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.NotificationActionLink == actionLink && notific.Notification_Heading.Trim().Equals(action)).SingleOrDefault();
                DataContext.Notifications.Remove(notification);
                return await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new SqlException(dbUpdateException, string.Format(ErrorMessageConstants.SqlDeleteExceptionMessage, string.Concat("notification for:", actionLink)));
            }
            catch (NotSupportedException notSupportedException)
            {
                throw new InfrastructureException(notSupportedException, ErrorMessageConstants.NotSupportedExceptionMessage);
            }
            catch (ObjectDisposedException disposedException)
            {
                throw new ServiceException(disposedException, ErrorMessageConstants.ObjectDisposedExceptionMessage);
            }
        }

        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        public NotificationDTO GetNotificationByUDPRN(int uDPRN)
        {
            string actionLink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN);
            Notification notification = DataContext.Notifications
                .Where(notific => notific.NotificationActionLink == actionLink).SingleOrDefault();
            NotificationDTO notificationDTO = new NotificationDTO();
            GenericMapper.Map(notification, notificationDTO);
            return notificationDTO;
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public bool CheckIfNotificationExists(int uDPRN, string action)
        {
            string notificationActionlink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN.ToString());
            if (DataContext.Notifications.AsNoTracking()
                .Any(notific => notific.NotificationActionLink.Equals(notificationActionlink) &&
                                  notific.Notification_Heading.Trim().Equals(action)))
            {
                return true;
            }

            return false;
        }
    }
}