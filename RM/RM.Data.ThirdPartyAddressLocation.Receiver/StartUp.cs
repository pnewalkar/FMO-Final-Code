using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ninject.Modules;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;

namespace RM.Data.ThirdPartyAddressLocation.Receiver
{
    public class StartUp : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>();
            Bind<IHttpHandler>().To<HttpHandler>();
            //Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            //Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope();
            //---Adding scope for all classes
            LogWriterFactory log = new LogWriterFactory();
            LogWriter logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);

            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}