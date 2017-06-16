using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RM.Common.Notification.WebAPI.BusinessService;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.Notification.WebAPI
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
                // This will push telemetry data through Application Insights pipeline faster,
                // allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

#if DEBUG
            SqlServerTypes.Utilities.LoadNativeAssemblies(System.IO.Path.Combine(env.ContentRootPath, "bin"));
#else
 SqlServerTypes.Utilities.LoadNativeAssemblies( env.ContentRootPath);
#endif

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddCors(
                options => options.AddPolicy("AllowCors", builder =>
                    {
                        builder
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                    }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                          new CamelCasePropertyNamesContractResolver();
            });

            //---Adding scope for all classes
            services.AddSingleton<ILoggingHelper, LoggingHelper>();
            services.AddSingleton<IExceptionHelper, ExceptionHelper>();

            // Infrastructure
            services.AddScoped<IDatabaseFactory<RMDBContext>, DatabaseFactory<RMDBContext>>();

            // DataServices services.AddScoped<IActionManagerDataService,
            // ActionManagerDataService>(); services.AddScoped<IUserRoleUnitDataService,
            // UserRoleUnitDataService>(); services.AddScoped<IUnitLocationDataService,
            // UnitLocationDataService>(); services.AddScoped<IAddressDataService,
            // AddressDataService>(); services.AddScoped<IDeliveryPointsDataService,
            // DeliveryPointsDataService>(); services.AddScoped<IFileProcessingLogDataService,
            // FileProcessingLogDataService>(); services.AddScoped<IAddressLocationDataService, AddressLocationDataService>();
            services.AddScoped<INotificationDataService, NotificationDataService>();
            //services.AddScoped<IDeliveryPointsDataService, DeliveryPointsDataService>();
            // services.AddScoped<IPostalAddressDataService, PostalAddressDataService>();
            services.AddScoped<IReferenceDataCategoryDataService, ReferenceDataCategoryDataService>();
            services.AddScoped<IDeliveryRouteDataService, DeliveryRouteDataService>();
            services.AddScoped<IScenarioDataService, ScenarioDataService>();
            services.AddScoped<IUnitLocationDataService, UnitLocationDataService>();
            services.AddScoped<IAddressLocationDataService, AddressLocationDataService>();
            services.AddScoped<IPostCodeDataService, PostCodeDataService>();
            services.AddScoped<IStreetNetworkDataService, StreetNetworkDataService>();
            services.AddScoped<INotificationDataService, NotificationDataService>();
            services.AddScoped<IUnitLocationDataService, UnitLocationDataService>();
            services.AddScoped<IOSRoadLinkDataService, OSRoadLinkDataService>();
            services.AddScoped<IFileProcessingLogDataService, FileProcessingLogDataService>();
            services.AddScoped<IReferenceDataDataService, ReferenceDataDataService>();
            services.AddScoped<IPostCodeSectorDataService, PostCodeSectorDataService>();
            services.AddScoped<IActionManagerDataService, ActionManagerDataService>();
            services.AddScoped<IUserRoleUnitDataService, UserRoleUnitDataService>();
            services.AddScoped<INotificationBusinessService, NotificationBusinessService>();

            // Others - Helper, Utils etc
            services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
            services.AddScoped<IHttpHandler, HttpHandler>();
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

            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            MapExceptionTypes(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=DeliveryPoints}/{action=Get}/{id?}");
            });

            WebHelpers.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }
    }
}