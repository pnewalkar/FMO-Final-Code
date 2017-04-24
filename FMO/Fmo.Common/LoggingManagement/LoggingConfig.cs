using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Fmo.Common.LoggingManagement
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
            TextFormatter formatter = new TextFormatter(resxManager.GetString("Logging_TextFormat", CultureInfo.CurrentCulture));

            // Listeners
            // var flatFileTraceListener = new FlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter);
            var rollingFlatFileTraceListener = new RollingFlatFileTraceListener(string.Concat(ConfigurationManager.AppSettings["LogFilePath"], ConfigurationManager.AppSettings["ErrorLogFileName"]), "----------------------------------------", "----------------------------------------", formatter, 20, "dd-MM-YYYY", RollFileExistsBehavior.Increment, RollInterval.None, 3);

            var eventLog = new EventLog(resxManager.GetString("Logging_LogName"), ".", resxManager.GetString("Logging_LogSource"));
            var eventLogTraceListener = new FormattedEventLogTraceListener(eventLog);

            // Build Configuration
            var config = new LoggingConfiguration();
            config.AddLogSource(resxManager.GetString("LogSource_LogSourceName"), SourceLevels.All, true).AddTraceListener(eventLogTraceListener);

            // config.LogSources["General"].AddTraceListener(flatFileTraceListener);
            config.LogSources[resxManager.GetString("LogSource_LogSourceName")].AddTraceListener(rollingFlatFileTraceListener);

            // Special Sources Configuration
            config.SpecialSources.LoggingErrorsAndWarnings.AddTraceListener(eventLogTraceListener);

            return config;
        }
    }
}