namespace RM.Integration.PostalAddress.PAFLoader
{
    using System.ServiceProcess;
    using Ninject;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.Integration.PostalAddress.PAFLoader.Utils.Interfaces;

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

            IPAFFileProcessUtility pafLoader = kernel.Get<IPAFFileProcessUtility>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();

#if DEBUG
            using (PAFImport myService = new PAFImport(pafLoader, loggingHelper, configurationHelper))
            {
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
#else
            ServiceBase[] servicesToRun = new ServiceBase[] { new PAFImport(pafLoader, loggingHelper, configurationHelper) };
            ServiceBase.Run(servicesToRun);
#endif

        }
    }
}