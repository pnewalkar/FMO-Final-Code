using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ninject.Modules;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Integration.PostalAddress.NYBLoader.Utils;
using RM.Integration.PostalAddress.NYBLoader.Utils.Interfaces;

namespace RM.Integration.PostalAddress.NYBLoader
{
    public class StartUp : NinjectModule
    {
        public override void Load()
        {
            Bind<INYBFileProcessUtility>().To<NYBFileProcessUtility>();
            Bind<IHttpHandler>().To<HttpHandler>();

            LogWriterFactory log = new LogWriterFactory();
            LogWriter logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);

            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope().WithConstructorArgument<LogWriter>(logWriter);

            Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}