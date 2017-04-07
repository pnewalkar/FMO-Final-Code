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
    /// This class should be inherited.
    /// </summary>
    public class ExceptionHelper : IExceptionHelper
    {
        // static ExceptionManager _exManager =   Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
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

        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy, out Exception execptionToThrow)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription(), out execptionToThrow);
        }

        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription());
        }
    }
}
