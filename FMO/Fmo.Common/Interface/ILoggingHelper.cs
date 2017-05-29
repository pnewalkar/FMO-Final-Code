using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.Interface
{
    public interface ILoggingHelper
    {
        /// <summary>
        /// Gets trace manager.
        /// </summary>
        TraceManager FmoTraceManager { get; }

        /// <summary>
        /// Logs Exception.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs Exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">Exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogInfo(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs Warning
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogWarn(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs Verbose.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogDebug(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogFatalError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void LogFatalError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle);
    }
}