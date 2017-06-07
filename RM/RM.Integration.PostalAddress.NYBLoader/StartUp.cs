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
            Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            Bind<IExceptionHelper>().To<ExceptionHelper>().InSingletonScope();
            Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
        }
    }
}