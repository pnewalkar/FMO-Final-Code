using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.LoggingMiddleware
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
        public TraceManager RMTraceManager
        {
            get
            {
                return traceManager;
            }
        }

        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="exception">exception</param>
        /// <param name="severity">severity</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void Log(Exception exception, TraceEventType severity, string category = Constants.DefaultLoggingCategory, int priority = Constants.DefaultLoggingPriority, int eventId = Constants.DefaultLoggingEventId, string title = Constants.DefaultLoggingTitle)
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

            Logger.Write(errorMessage, category, priority, eventId, severity, title);
        }

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
        public void Log(string message, TraceEventType severity, Exception exception = null, string category = Constants.DefaultLoggingCategory, int priority = Constants.DefaultLoggingPriority, int eventId = Constants.DefaultLoggingEventId, string title = Constants.DefaultLoggingTitle)
        {
            string errorMessage = string.Empty;
            if (exception != null)
            {
                string stackStrace = exception.StackTrace;
                if (exception.InnerException != null)
                {
                    errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message + Environment.NewLine + "StackTrace: " + stackStrace;
                }
                else
                {
                    errorMessage = message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "StackTrace: " + stackStrace;
                }
            }
            else
            {
                errorMessage = message;
            }

            Logger.Write(errorMessage, category, priority, eventId, severity, title);
        }
    }
}