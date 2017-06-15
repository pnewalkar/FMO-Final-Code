using System.ServiceProcess;
using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;

namespace RM.Data.PostalAddress.PAFReceiver
{
    /// <summary>
    /// Entry point for PAF receiver service
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            IKernel kernel = new StandardKernel(new StartUp());
            IMessageBroker<PostalAddressBatchDTO> msgPAF = kernel.Get<IMessageBroker<PostalAddressBatchDTO>>();
            IHttpHandler httpHandler = kernel.Get<IHttpHandler>();
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();

#if DEBUG
            using (PAFReceiver myService = new PAFReceiver(msgPAF, httpHandler, configurationHelper, loggingHelper))
            {
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
#else

            ServiceBase[] servicesToRun = new ServiceBase[] { new PAFReceiver(msgPAF, httpHandler, configurationHelper, loggingHelper) };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}