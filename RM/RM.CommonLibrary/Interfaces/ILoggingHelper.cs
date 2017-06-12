using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.LoggingMiddleware
{
    public interface ILoggingHelper
    {
        /// <summary>
        /// Gets trace manager.
        /// </summary>
      TraceManager RMTraceManager { get; }

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="severity">severity</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void Log(Exception exception, TraceEventType severity, string category = Constants.DefaultLoggingCategory, int priority = Constants.DefaultLoggingPriority, int eventId = Constants.DefaultLoggingEventId, string title = Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="severity">severity</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void Log(string message, TraceEventType severity, Exception exception = null, string category = Constants.DefaultLoggingCategory, int priority = Constants.DefaultLoggingPriority, int eventId = Constants.DefaultLoggingEventId, string title = Constants.DefaultLoggingTitle);
    }
}