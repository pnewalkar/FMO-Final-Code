using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;
using System.ServiceProcess;

namespace RM.Data.ThirdPartyAddressLocation.Receiver
{
    /// <summary>
    /// Entry point for receiver service
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            IKernel kernel = new StandardKernel(new StartUp());
            IMessageBroker<AddressLocationUSRDTO> msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
            IHttpHandler httpHandler = kernel.Get<IHttpHandler>();
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();

            ServiceBase[] servicesToRun = new ServiceBase[] { new ThirdPartyAddressLocationReceiver(msgUSR, configurationHelper, loggingHelper, httpHandler) };
            ServiceBase.Run(servicesToRun);
        }
    }
}
