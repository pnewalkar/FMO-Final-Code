using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// To interact with the Notification database entity.
    /// </summary>
    public class NotificationDataService : DataServiceBase<Notification, RMDBContext>, INotificationDataService
    {
        private const string USRNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";

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
                int saveChangesAsync = default(int);
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    Notification newNotification = new Notification();
                    GenericMapper.Map(notificationDTO, newNotification);
                    DataContext.Notifications.Add(newNotification);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    saveChangesAsync = await DataContext.SaveChangesAsync();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    loggingHelper.Log(dbUpdateException, TraceEventType.Error);
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("notification for:", notificationDTO.NotificationActionLink)));
                }
                catch (NotSupportedException notSupportedException)
                {
                    loggingHelper.Log(notSupportedException, TraceEventType.Error);
                    notSupportedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
                }
                catch (ObjectDisposedException disposedException)
                {
                    loggingHelper.Log(disposedException, TraceEventType.Error);
                    disposedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
                }
                return saveChangesAsync;
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
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                int deleteCount = default(int);
                string actionLink = string.Format(USRNOTIFICATIONLINK, uDPRN);
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
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlDeleteException, string.Concat("notification for:", actionLink)));
                }
                catch (NotSupportedException notSupportedException)
                {
                    notSupportedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
                }
                catch (ObjectDisposedException disposedException)
                {
                    disposedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
                }
                finally
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
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
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNotificationByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string actionLink = string.Format(USRNOTIFICATIONLINK, uDPRN);
                Notification notification = await DataContext.Notifications
                    .Where(notific => notific.NotificationActionLink == actionLink).SingleOrDefaultAsync();
                NotificationDTO notificationDTO = new NotificationDTO();
                GenericMapper.Map(notification, notificationDTO);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return notificationDTO;
            }
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckIfNotificationExists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string notificationActionlink = string.Format(USRNOTIFICATIONLINK, uDPRN.ToString());
                bool notificationExists = await DataContext.Notifications.AsNoTracking()
                .AnyAsync(notific => notific.NotificationActionLink.Equals(notificationActionlink) &&
                                  notific.Notification_Heading.Trim().Equals(action));
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return notificationExists;
            }
        }

        public async Task<bool> UpdateNotificationByUDPRN(int uDPRN, string oldAction, string newAction)
        {
            string notificationActionlink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN.ToString());
            bool returnVal = false;

            Notification notification = await DataContext.Notifications.Where(notific => notific.NotificationActionLink == notificationActionlink && notific.Notification_Heading.Equals(oldAction)).SingleOrDefaultAsync();
            notification.Notification_Heading = newAction;
            await DataContext.SaveChangesAsync();

            returnVal = true;

            return returnVal;
        }

        public async Task<bool> UpdateNotificationMessageByUDPRN(int uDPRN, string action, string message)
        {
            string notificationActionlink = string.Format(Constants.USRNOTIFICATIONLINK, uDPRN.ToString());
            bool returnVal = false;

            Notification notification = await DataContext.Notifications.Where(notific => notific.NotificationActionLink == notificationActionlink && notific.Notification_Heading.Equals(action)).SingleOrDefaultAsync();
            notification.Notification_Message = message;
            await DataContext.SaveChangesAsync();

            returnVal = true;

            return returnVal;
        }

    }
}