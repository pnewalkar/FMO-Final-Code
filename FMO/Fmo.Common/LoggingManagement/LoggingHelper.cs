using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.LoggingManagement
{
    public class LoggingHelper : ILoggingHelper
    {
        private static LogWriter logWriter = null;

        public LoggingHelper()
        {
            LoggingConfiguration loggingConfiguration = LoggingConfig.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
            Logger.SetLogWriter(logWriter, false);

            if (!Directory.Exists(@"D:\Temp"))
            {
                Directory.CreateDirectory(@"D:\Temp");
            }
        }

        public void LogError(Exception exception)
        {
            string errorMessage = default(string);
            if (exception.InnerException != null)
            {
                errorMessage += "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message;
            }

            Logger.Write(errorMessage, LoggingCategory.Exception.GetDescription(), 0, 0, TraceEventType.Error);
        }

        public void LogError(string message, Exception exception)
        {
            string errorMessage = default(string);
            if (exception.InnerException != null)
            {
                errorMessage += message + Environment.NewLine + "Exception: " + exception.Message + Environment.NewLine + "Inner Exception: " + exception.InnerException.Message;
            }

            Logger.Write(errorMessage, LoggingCategory.Exception.GetDescription(), 0, 0, TraceEventType.Error);
        }

        public void LogInfo(string message)
        {
            Logger.Write(message, LoggingCategory.General.GetDescription(), 0, 0, TraceEventType.Warning);
        }

        public void LogWarn(string message)
        {
            Logger.Write(message, LoggingCategory.General.GetDescription(), 0, 0, TraceEventType.Information);
        }
    }
}
