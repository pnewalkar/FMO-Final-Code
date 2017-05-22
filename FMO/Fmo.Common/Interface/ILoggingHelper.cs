using System;

namespace Fmo.Common.Interface
{
    public interface ILoggingHelper
    {
        /// <summary>
        /// Logs Exception.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs Exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">Exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogInfo(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs Warning
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogWarn(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs Verbose.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogDebug(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogFatalError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        void LogFatalError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId);
    }
}