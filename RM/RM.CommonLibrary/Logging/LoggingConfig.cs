using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.LoggingMiddleware
{
    /// <summary>
    /// Logging configuration
    /// </summary>
    public static class LoggingConfig
    {
        /// <summary>
        /// Builds Programmatic Configuration for Logging
        /// </summary>
        /// <returns>LoggingConfiguration</returns>
        public static LoggingConfiguration BuildProgrammaticConfig()
        {
            ResourceManager resxManager = new ResourceManager(ConfigurationManager.AppSettings["FmoMessages_ResourceFileName"], Assembly.GetExecutingAssembly());

            // Formatters
            TextFormatter formatter = new TextFormatter(ErrorConstants.Logging_TextFormat);

            // Listeners
            // var flatFileTraceListener = new FlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter);
            var rollingFlatFileTraceListener = new RollingFlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter, 20, "yyyy-MM-dd", RollFileExistsBehavior.Increment, RollInterval.None, 3);

            var eventLog = new EventLog(ErrorConstants.Logging_LogName, ".", ErrorConstants.Logging_LogSource);
            var eventLogTraceListener = new FormattedEventLogTraceListener(eventLog);

            // Build Configuration
            var config = new LoggingConfiguration();
            config.AddLogSource(ErrorConstants.LogSource_LogSourceName, SourceLevels.All, true).AddTraceListener(eventLogTraceListener);

            // config.LogSources["General"].AddTraceListener(flatFileTraceListener);
            config.LogSources[ErrorConstants.LogSource_LogSourceName].AddTraceListener(rollingFlatFileTraceListener);

            // Special Sources Configuration
            config.SpecialSources.LoggingErrorsAndWarnings.AddTraceListener(eventLogTraceListener);

            return config;
        }
    }
}