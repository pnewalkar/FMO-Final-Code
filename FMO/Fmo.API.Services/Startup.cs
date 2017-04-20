using Fmo.API.Services.MiddlerWare;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.ConfigurationManagement;
using Fmo.Common.EmailManagement;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.Interface;
using Fmo.Common.LoggingManagement;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Helpers;
using Fmo.Helpers.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fmo.API.Services
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

#if DEBUG
            SqlServerTypes.Utilities.LoadNativeAssemblies(System.IO.Path.Combine(env.ContentRootPath, "bin"));
#else
 SqlServerTypes.Utilities.LoadNativeAssemblies( env.ContentRootPath);
#endif
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                    })
            );

            services.AddMvc();

            //---Adding scope for all classes
            services.AddSingleton<IExceptionHelper, ExceptionHelper>();
            services.AddSingleton<ILoggingHelper, LoggingHelper>();
            services.AddSingleton<IEmailHelper, EmailHelper>();

            //Infrastructure
            services.AddTransient<IUnitOfWork<FMODBContext>, UnitOfWork<FMODBContext>>();
            services.AddTransient<IDatabaseFactory<FMODBContext>, DatabaseFactory<FMODBContext>>();

            //BusinessServices
            services.AddTransient<IDeliveryPointBusinessService, DeliveryPointBusinessService>();
            services.AddTransient<ISearchBusinessService, SearchBusinessService>();
            services.AddTransient<IPostalAddressBusinessService, PostalAddressBusinessService>();
            services.AddTransient<IDeliveryRouteBusinessService, DeliveryRouteBusinessService>();
            services.AddTransient<IAccessLinkBusinessService, AccessLinkBussinessService>();
            services.AddTransient<IActionManagerBussinessService, ActionManagerBussinessService>();
            services.AddTransient<IUserRoleUnitBussinessService, UserRoleUnitBussinessService>();
            services.AddTransient<IUSRBusinessService, USRBusinessService>();
            services.AddTransient<IAccessActionBusinessService, AccessActionBussinessService>();
            services.AddTransient<IUnitLocationBusinessService, UnitLocationBusinessService>();
            //Repositories
            services.AddTransient<IAccessActionRepository, AccessActionRepository>();
            services.AddTransient<IAccessLinkRepository, AccessLinkRepository>();
            services.AddTransient<IRoadNameBusinessService, RoadNameBussinessService>();
            services.AddTransient<IRoadNameRepository, RoadNameRepository>();

            //Repositories
            services.AddTransient<IDeliveryPointsRepository, DeliveryPointsRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IReferenceDataCategoryRepository, ReferenceDataCategoryRepository>();
            services.AddTransient<IDeliveryRouteRepository, DeliveryRouteRepository>();
            services.AddTransient<IScenarioRepository, ScenarioRepository>();
            services.AddTransient<IUnitLocationRepository, UnitLocationRepository>();
            services.AddTransient<IAddressLocationRepository, AddressLocationRepository>();
            services.AddTransient<IPostalAddressRepository, PostalAddressRepository>();
            services.AddTransient<IPostCodeRepository, PostCodeRepository>();
            services.AddTransient<IStreetNetworkRepository, StreetNetworkRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IUnitLocationRepository, UnitLocationRepository>();

            //services.AddTransient<IFileProcessingLogRepository, FileProcessingLogRepository>();
            services.AddTransient<IReferenceDataRepository, ReferenceDataRepository>();
            services.AddTransient<IPostCodeSectorRepository, PostCodeSectorRepository>();
            services.AddTransient<IActionManagerRepository, ActionManagerRepository>();
            services.AddTransient<IUserRoleUnitRepository, UserRoleUnitRepository>();

            //Others - Helper, Utils etc
            services.AddTransient<ILoggingHelper, LoggingHelper>();
            services.AddTransient<ICreateOtherLayersObjects, CreateOtherLayerObjects>();
            services.AddTransient<IFileProcessingLogRepository, FileProcessingLogRepository>();
            services.AddTransient<IConfigurationHelper, ConfigurationHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("AllowCors");

            ConfigureAuth(app);
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=DeliveryPoints}/{action=Get}/{id?}");
            });
        }
    }
}