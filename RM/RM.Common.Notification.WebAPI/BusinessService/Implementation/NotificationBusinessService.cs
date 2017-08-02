using System.Diagnostics;
using System.Threading.Tasks;
using RM.Common.Notification.WebAPI.DataService.Interface;
using RM.Common.Notification.WebAPI.DTO;
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
                    NotificationDataDTO notificationDataDTO = new NotificationDataDTO
                    {
                        ID = notificationDTO.ID,
                        Notification_Heading = notificationDTO.Notification_Heading,
                        Notification_Message = notificationDTO.Notification_Message,
                        NotificationDueDate = notificationDTO.NotificationDueDate,
                        NotificationActionLink = notificationDTO.NotificationActionLink,
                        NotificationSource = notificationDTO.NotificationSource,
                        PostcodeDistrict = notificationDTO.PostcodeDistrict,
                        PostcodeSector = notificationDTO.PostcodeSector,
                        NotificationTypeGUID = notificationDTO.NotificationType_GUID,
                        NotificationPriorityGUID = notificationDTO.NotificationPriority_GUID
                    };

                    return await notificationDataService.AddNewNotification(notificationDataDTO);
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.CheckIfNotificationExists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                var checkIfNotificationExists = await notificationDataService.CheckIfNotificationExists(uDPRN, action);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return checkIfNotificationExists;
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetNotificationByUDPRN"))
            {
                NotificationDTO notificationDTO = null;
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                var getNotificationByUDPRN = await notificationDataService.GetNotificationByUDPRN(uDPRN);
                if (getNotificationByUDPRN != null)
                {
                    notificationDTO = new NotificationDTO
                    {
                        ID = getNotificationByUDPRN.ID,
                        Notification_Heading = getNotificationByUDPRN.Notification_Heading,
                        Notification_Message = getNotificationByUDPRN.Notification_Message,
                        NotificationDueDate = getNotificationByUDPRN.NotificationDueDate,
                        NotificationActionLink = getNotificationByUDPRN.NotificationActionLink,
                        NotificationSource = getNotificationByUDPRN.NotificationSource,
                        PostcodeDistrict = getNotificationByUDPRN.PostcodeDistrict,
                        PostcodeSector = getNotificationByUDPRN.PostcodeSector,
                        NotificationType_GUID = getNotificationByUDPRN.NotificationTypeGUID,
                        NotificationPriority_GUID = getNotificationByUDPRN.NotificationPriorityGUID
                    };
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return notificationDTO;
            }
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