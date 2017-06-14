﻿using System;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.ResourceFile;
using System.Data.Entity.Infrastructure;
using RM.CommonLibrary.ExceptionMiddleware;
using System.Data.Entity;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using System.Reflection;
using System.Diagnostics;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// To interact with the Notification database entity.
    /// </summary>
    public class NotificationDataService : DataServiceBase<Notification, RMDBContext>, INotificationDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        public NotificationDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.AddNewNotification"))
            {
                string methodName = MethodHelper.GetRealMethodFromAsyncMethod(MethodBase.GetCurrentMethod()).Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                Notification newNotification = new Notification();
                GenericMapper.Map(notificationDTO, newNotification);
                DataContext.Notifications.Add(newNotification);
                return await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorMessageIds.Err_SqlAddException, string.Concat("notification for:", notificationDTO.NotificationActionLink)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorMessageIds.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new ServiceException(disposedException, ErrorMessageIds.Err_ObjectDisposedException);
            }
                finally
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
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
            using (loggingHelper.RMTraceManager.StartTrace("DataService.DeleteNotificationbyUDPRNAndAction"))
            {
                string methodName = MethodHelper.GetRealMethodFromAsyncMethod(MethodBase.GetCurrentMethod()).Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                int deleteCount = default(int);
            string actionLink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN);
            try
            {
                Notification notification = DataContext.Notifications.Where(notific => notific.NotificationActionLink == actionLink && notific.Notification_Heading.Trim().Equals(action)).SingleOrDefault();
                if (notification != null)
                {
                    DataContext.Notifications.Remove(notification);
                    deleteCount = await DataContext.SaveChangesAsync();
                }

                return deleteCount;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorMessageIds.Err_SqlDeleteException, string.Concat("notification for:", actionLink)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorMessageIds.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new ServiceException(disposedException, ErrorMessageIds.Err_ObjectDisposedException);
            }
                finally
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
        }

        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        public async Task<NotificationDTO> GetNotificationByUDPRN(int uDPRN)
        {
            string actionLink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN);
            Notification notification = await DataContext.Notifications
                .Where(notific => notific.NotificationActionLink == actionLink).SingleOrDefaultAsync();
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
        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            string notificationActionlink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN.ToString());
            return await DataContext.Notifications.AsNoTracking()
                .AnyAsync(notific => notific.NotificationActionLink.Equals(notificationActionlink) &&
                                  notific.Notification_Heading.Trim().Equals(action));
        }
    }
}