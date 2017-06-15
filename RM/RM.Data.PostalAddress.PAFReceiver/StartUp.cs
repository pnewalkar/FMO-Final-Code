using Ninject.Modules;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;

namespace RM.Data.PostalAddress.PAFReceiver
{
    public class StartUp : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBroker<PostalAddressBatchDTO>>().To<MessageBroker<PostalAddressBatchDTO>>();
            Bind<IHttpHandler>().To<HttpHandler>();
            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope();
            Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}