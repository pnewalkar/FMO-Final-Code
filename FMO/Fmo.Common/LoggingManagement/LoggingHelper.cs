﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Fmo.Common.Interface;
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
            if (!Directory.Exists(ConfigurationManager.AppSettings["LogFilePath"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFilePath"]);
            }

            LoggingConfiguration loggingConfiguration = LoggingConfig.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
            Logger.SetLogWriter(logWriter, false);
        }

        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="exception">exception</param>
        public void LogError(Exception exception)
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

            Logger.Write(errorMessage, "General", 0, 0, TraceEventType.Error);
        }

        /// <summary>
        /// Logs Error
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        public void LogError(string message, Exception exception)
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

            Logger.Write(errorMessage, "General", 0, 0, TraceEventType.Error);
        }

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">message</param>
        public void LogInfo(string message)
        {
            Logger.Write(message, "General", 0, 0, TraceEventType.Warning);
        }

        /// <summary>
        /// Logs Information
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="enableLogging">Is Logging enabled</param>
        public void LogInfo(string message, bool enableLogging)
        {
            if (enableLogging)
            {
                Logger.Write(message, "General", 0, 0, TraceEventType.Information);
            }
        }

        /// <summary>
        /// Logs Warning
        /// </summary>
        /// <param name="message">message</param>
        public void LogWarn(string message)
        {
            Logger.Write(message, "General", 0, 0, TraceEventType.Information);
        }
    }
}