using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.Common.Notification.WebAPI.BusinessService
{
    public class NotificationBusinessService : INotificationBusinessService
    {
        private INotificationDataService notificationDataService = default(INotificationDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public NotificationBusinessService(INotificationDataService notificationDataService, ILoggingHelper loggingHelper)
        {
            this.notificationDataService = notificationDataService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.AddNewNotification"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    return await notificationDataService.AddNewNotification(notificationDTO);
                }
                finally
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.DeleteNotificationbyUDPRNAndAction"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    return await notificationDataService.DeleteNotificationbyUDPRNAndAction(uDPRN, action);
                }
                finally
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
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
            return await notificationDataService.GetNotificationByUDPRN(uDPRN);
        }
    }
}