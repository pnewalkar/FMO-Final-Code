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
        private static LogWriter logWriter = null;

        public LoggingHelper()
        {          

            IConfigurationSource source = ConfigurationSourceFactory.Create();
            LogWriterFactory log = new LogWriterFactory(source);
            logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);
        }

        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
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

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Error);
        }

        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
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

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Error);
        }

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogInfo(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Information);
        }

        /// <summary>
        /// Logs Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogWarn(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Warning);
        }

        /// <summary>
        /// Logs Verbose.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogDebug(string message, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
        {
            Logger.Write(message, category, priority, eventId, TraceEventType.Verbose);
        }

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogFatalError(Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
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

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Critical);
        }

        /// <summary>
        /// Logs fatal Error
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogFatalError(string message, Exception exception, string category = Constants.Constants.DefaultLoggingCategory, int priority = Constants.Constants.DefaultLoggingPriority, int eventId = Constants.Constants.DefaultLoggingEventId)
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

            Logger.Write(errorMessage, category, priority, eventId, TraceEventType.Critical);
        }
    }
}