using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Fmo.Common.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.LoggingManagement
{
    /// <summary>
    /// Logging Helper class
    /// </summary>
    public class LoggingHelper : ILoggingHelper
    {
        private static TraceManager traceManager = null;
        private static LogWriter logWriter = null;

        public LoggingHelper()
        {
            IConfigurationSource source = ConfigurationSourceFactory.Create();
            LogWriterFactory log = new LogWriterFactory(source);
            logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);
            traceManager = new TraceManager(logWriter);
        }

        /// <summary>
        /// Gets trace manager.
        /// </summary>
        public TraceManager FmoTraceManager
        {
            get
            {
                return traceManager;
            }
        }


        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            string errorMessage = string.Empty;
            string stackStrace = exception.StackTrace;
            if (exception.InnerException != null)
            {
                errorMessage = "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }
            else
            {
                errorMessage = "Exception: " + exception.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Error, title);
        }

        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            string errorMessage = string.Empty;
            string stackStrace = exception.StackTrace;
            if (exception.InnerException != null)
            {
                errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }
            else
            {
                errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Error, title);
        }

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogInfo(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Information, title);
        }

        /// <summary>
        /// Logs Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogWarn(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Warning, title);
        }

        /// <summary>
        /// Logs Verbose.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogDebug(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Verbose, title);
        }

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogFatalError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            string errorMessage = string.Empty;
            string stackStrace = exception.StackTrace;
            if (exception.InnerException != null)
            {
                errorMessage = "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }
            else
            {
                errorMessage = "Exception: " + exception.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Critical, title);
        }

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void LogFatalError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId, string title = Constants.Constants.DefaultLoggingTitle)
        {
            string errorMessage = string.Empty;
            string stackStrace = exception.StackTrace;
            if (exception.InnerException != null)
            {
                errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }
            else
            {
                errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "StackTrace: " + stackStrace;
            }

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Critical, title);
        }
    }
}