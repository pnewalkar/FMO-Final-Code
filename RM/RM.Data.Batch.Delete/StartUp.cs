using Ninject.Modules;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;

namespace RM.DataManagement.Batch.Delete
{
    public class StartUp : NinjectModule
    {
        public override void Load()
        {
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
