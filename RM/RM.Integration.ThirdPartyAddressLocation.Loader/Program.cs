using System.ServiceProcess;
using Ninject;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.ConfigurationMiddleware;

namespace RM.Integration.ThirdPartyAddressLocation.Loader
{
    /// <summary>
    /// Entry Point for FileLoader service
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            IKernel kernel = new StandardKernel(new StartUp());
            IThirdPartyFileProcessUtility usrLoader = kernel.Get<IThirdPartyFileProcessUtility>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();

            ServiceBase[] servicesToRun = new ServiceBase[] { new ThirdPartyImport(usrLoader, loggingHelper, configurationHelper) };
            ServiceBase.Run(servicesToRun);
        }
    }
}
