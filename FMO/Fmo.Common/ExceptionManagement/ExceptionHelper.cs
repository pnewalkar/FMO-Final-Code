using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Fmo.Common.LoggingManagement;

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
                //IConfigurationSource config = ConfigurationSourceFactory.Create();
                //ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
                //ExceptionManager exceptionManager = factory.CreateManager();
                //ExceptionPolicy.SetExceptionManager(exceptionManager);

                LoggingConfiguration loggingConfiguration = LoggingConfig.BuildProgrammaticConfig();
                LogWriter logWriter = new LogWriter(loggingConfiguration);
                ExceptionManager exceptionManager = ExceptionHandlingConfiguration.BuildExceptionHandlingConfiguration(logWriter);
                ExceptionPolicy.SetExceptionManager(exceptionManager);
                isInitialized = true;

                if (!Directory.Exists(@"C:\Temp"))
                {
                    Directory.CreateDirectory(@"C:\Temp");
                }
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
