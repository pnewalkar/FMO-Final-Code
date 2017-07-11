using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.LoggingMiddleware
{
    /// <summary>
    /// Logging Helper class
    /// </summary>
    public class LoggingHelper : ILoggingHelper
    {
        private static IRMTraceManager traceManager = null;

        public LoggingHelper(LogWriter logWriter)
        {
            traceManager = new RMTraceManager(logWriter);
        }

        /// <summary>
        /// Gets trace manager.
        /// </summary>
        public IRMTraceManager RMTraceManager
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
        public void Log(Exception exception, TraceEventType severity, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle)
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
        public void Log(string message, TraceEventType severity, Exception exception = null, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle)
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


        /// <summary>
        /// General Log for method entry.
        /// </summary>
        /// <param name="methodName">MethodName</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogMethodEntry(string methodName, int priority, int eventId)
        {
            string method = methodName;
            string message = methodName + " : Method Execution Started";

            Log(message, TraceEventType.Verbose, null, LoggerTraceConstants.Category, priority, eventId, LoggerTraceConstants.Title);
        }

        /// <summary>
        /// General Log for method exit.
        /// </summary>
        /// <param name="methodName">Method Name</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        public void LogMethodExit(string methodName, int priority, int eventId)
        {
            string method = methodName;
            string message = methodName + " : Method Execution Completed";

            Log(message, TraceEventType.Verbose, null, LoggerTraceConstants.Category, priority, eventId, LoggerTraceConstants.Title);
        }

        /// <summary>
        /// Starts trace.
        /// </summary>
        /// <param name="methodName">message</param>
        /// <param name="severity">severity</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void StartTrace(string methodName, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle, TraceEventType severity = TraceEventType.Verbose)
        {
            Logger.Write(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, category, priority, eventId, severity, title);
        }

        /// <summary>
        /// stiops the trace.
        /// </summary>
        /// <param name="methodName">message</param>
        /// <param name="severity">severity</param>
        /// <param name="exception">exception</param>
        /// <param name="category">Category</param>
        /// <param name="priority">Priority</param>
        /// <param name="eventId">Event ID</param>
        /// <param name="title">Title</param>
        public void StopTrace(string methodName, string category = LoggerTraceConstants.DefaultLoggingCategory, int priority = LoggerTraceConstants.DefaultLoggingPriority, int eventId = LoggerTraceConstants.DefaultLoggingEventId, string title = LoggerTraceConstants.DefaultLoggingTitle, TraceEventType severity = TraceEventType.Verbose)
        {
            Logger.Write(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, category, priority, eventId, severity, title);
        }
    }
}