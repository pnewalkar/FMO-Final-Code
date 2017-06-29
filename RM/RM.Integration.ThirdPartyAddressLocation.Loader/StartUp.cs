using Ninject.Modules;
using RM.CommonLibrary.MessageBrokerMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils;
using RM.CommonLibrary.EntityFramework.DataService;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RM.Integration.ThirdPartyAddressLocation.Loader
{
    public class StartUp : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>();
            Bind<IThirdPartyFileProcessUtility>().To<ThirdPartyFileProcessUtility>();
          
            LogWriterFactory log = new LogWriterFactory();
            LogWriter logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);

            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);

            Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
            Bind<IFileProcessingLogDataService>().To<FileProcessingLogDataService>();
            Bind<IFileMover>().To<FileMover>();
        }
    }
}