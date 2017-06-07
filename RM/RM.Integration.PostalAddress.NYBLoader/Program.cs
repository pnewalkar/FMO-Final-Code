using System.ServiceProcess;
using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Integration.PostalAddress.NYBLoader.Utils.Interfaces;

namespace RM.Integration.PostalAddress.NYBLoader
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
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();
            INYBFileProcessUtility nybLoader = kernel.Get<INYBFileProcessUtility>();
            ServiceBase[] servicesToRun = new ServiceBase[] { new NYBImport(nybLoader, loggingHelper, configurationHelper) };
            ServiceBase.Run(servicesToRun);
        }
    }
}