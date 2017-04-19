using System;
using System.Configuration;
using System.IO;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.Common.LoggingManagement;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.ExceptionManagement
{
    /// <summary>
    /// Enterprise library exception policy helper class for managing and logging exceptions.
    /// </summary>
    public class ExceptionHelper : IExceptionHelper
    {
        private static bool isInitialized = false;

        public ExceptionHelper()
        {
            if (!isInitialized)
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["LogFilePath"]))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFilePath"]);
                }

                LoggingConfiguration loggingConfiguration = LoggingConfig.BuildProgrammaticConfig();
                LogWriter logWriter = new LogWriter(loggingConfiguration);
                ExceptionManager exceptionManager = ExceptionHandlingConfiguration.BuildExceptionHandlingConfiguration(logWriter);
                ExceptionPolicy.SetExceptionManager(exceptionManager);
                isInitialized = true;
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="execptionToThrow">The execption to throw.</param>
        /// <returns>bool</returns>
        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy, out Exception execptionToThrow)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription(), out execptionToThrow);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>bool</returns>
        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription());
        }
    }
}
