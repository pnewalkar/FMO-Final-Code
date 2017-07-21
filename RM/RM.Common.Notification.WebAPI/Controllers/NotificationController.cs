using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.Common.Notification.WebAPI.BusinessService;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.Common.Notification.WebAPI.Controllers
{
    [Route("api/notificationmanager")]
    public class NotificationController : RMBaseController
    {
        private INotificationBusinessService notificationBusinessService = default(INotificationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public NotificationController(INotificationBusinessService notificationBusinessService, ILoggingHelper loggingHelper)
        {
            this.notificationBusinessService = notificationBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        [Route("notifications/add")]
        [HttpPost]
        public async Task<IActionResult> AddNewNotification([FromBody]NotificationDTO notificationDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.AddNewNotification"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    int status = await notificationBusinessService.AddNewNotification(notificationDTO);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(status);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        [Route("notifications/check/{udprn}/{actionname}")]
        [HttpGet]
        public async Task<IActionResult> CheckIfNotificationExists(int udprn, string actionname)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.CheckIfNotificationExists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    bool notificationExists = await notificationBusinessService.CheckIfNotificationExists(udprn, actionname);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(notificationExists);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        [Route("notifications/delete/{udprn}/{actionname}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNotificationbyUDPRNAndAction(int udprn, string actionname)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.DeleteNotificationbyUDPRNAndAction"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    int status = await notificationBusinessService.DeleteNotificationbyUDPRNAndAction(udprn, actionname);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(status);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Get the notification details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>NotificationDTO object</returns>
        [Route("notifications/{udprn}")]
        [HttpGet]
        public async Task<IActionResult> GetNotificationByUDPRN(int udprn)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNotificationByUDPRN"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    NotificationDTO notificationDTo = await notificationBusinessService.GetNotificationByUDPRN(udprn);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NotificationAPIPriority, LoggerTraceConstants.NotificationControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(notificationDTo);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        [Route("notifications/location/{udprn}/{oldAction}")]
        [HttpPut]
        public async Task<IActionResult> UpdateNotificationByUDPRN(int udprn, string oldAction, [FromBody]string newAction)
        {
            try
            {
                bool isUpdated = await notificationBusinessService.UpdateNotificationByUDPRN(udprn, oldAction, newAction);
                return Ok(isUpdated);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }


        [Route("notifications/postaladdress/{udprn}/{newaction}")]
        [HttpPut]
        public async Task<IActionResult> UpdateNotificationMessageByUDPRN(int udprn, string newaction, [FromBody]string message)
        {
            try
            {
                bool isUpdated = await notificationBusinessService.UpdateNotificationMessageByUDPRN(udprn, newaction, message);
                return Ok(isUpdated);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
    }
}