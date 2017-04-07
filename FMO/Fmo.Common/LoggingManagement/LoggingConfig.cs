using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Fmo.Common.LoggingManagement
{
    public class LoggingConfig
    {
        public static LoggingConfiguration BuildProgrammaticConfig()
        {
            // Formatters
            TextFormatter formatter = new TextFormatter("Timestamp: {timestamp}{newline}Message: {message}{newline}Category: {category}{newline}Priority: {priority}{newline}EventId: {eventid}{newline}Severity: {severity}{newline}Title:{title}{newline}Machine: {localMachine}{newline}App Domain: {localAppDomain}{newline}ProcessId: {localProcessId}{newline}Process Name: {localProcessName}{newline}Thread Name: {threadName}{newline}Win32 ThreadId:{win32ThreadId}{newline}Extended Properties: {dictionary({key} - {value}{newline})}");

            // Listeners
            // var flatFileTraceListener = new FlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter);
            var rollingFlatFileTraceListener = new RollingFlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter, 20, "yyyy-MM-dd", RollFileExistsBehavior.Increment, RollInterval.None, 3);

            var eventLog = new EventLog("Application", ".", "FMO Logging");
            var eventLogTraceListener = new FormattedEventLogTraceListener(eventLog);

            // Build Configuration
            var config = new LoggingConfiguration();
            config.AddLogSource("General", SourceLevels.All, true).AddTraceListener(eventLogTraceListener);

            // config.LogSources["General"].AddTraceListener(flatFileTraceListener);
            config.LogSources["General"].AddTraceListener(rollingFlatFileTraceListener);

            // Special Sources Configuration
            config.SpecialSources.LoggingErrorsAndWarnings.AddTraceListener(eventLogTraceListener);

            return config;
        }
    }
}
