using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;

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

#if DEBUG
            using (ThirdPartyImport myService = new ThirdPartyImport(usrLoader, loggingHelper, configurationHelper))
            {
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
#else
            ServiceBase[] servicesToRun = new ServiceBase[] { new ThirdPartyImport(usrLoader, loggingHelper, configurationHelper) };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}