namespace RM.Integration.PostalAddress.PAFLoader
{
    using Ninject.Modules;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.CommonLibrary.MessageBrokerMiddleware;
    using RM.Integration.PostalAddress.PAFLoader.Utils;
    using Utils.Interfaces;

    public class StartUp : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IMessageBroker<PostalAddressBatchDTO>>().To<MessageBroker<PostalAddressBatchDTO>>();
            this.Bind<IPAFFileProcessUtility>().To<PAFFileProcessUtility>();
            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope();
            this.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}