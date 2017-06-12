namespace RM.Integration.PostalAddress.PAFLoader
{
    using Microsoft.Practices.EnterpriseLibrary.Logging;
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
            this.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>();
            this.Bind<IPAFFileProcessUtility>().To<PAFFileProcessUtility>();

            //---Adding scope for all classes
            LogWriterFactory log = new LogWriterFactory();
            LogWriter logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);

            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            this.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}