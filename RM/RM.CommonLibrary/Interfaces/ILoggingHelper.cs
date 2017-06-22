using System;
using System.Diagnostics;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.LoggingMiddleware
{
    public interface ILoggingHelper
    {
        /// <summary>
        /// Gets trace manager.
        /// </summary>
        IRMTraceManager RMTraceManager { get; }

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="severity">severity</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void Log(Exception exception, TraceEventType severity, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle);

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
        void Log(string message, TraceEventType severity, Exception exception = null, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle);

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="methodName">message</param>
        /// <param name="severity">severity</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void StartTrace(string methodName, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle, TraceEventType severity = TraceEventType.Verbose);

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="methodName">message</param>
        /// <param name="severity">severity</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        void StopTrace(string methodName, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle, TraceEventType severity = TraceEventType.Verbose);
    }
}