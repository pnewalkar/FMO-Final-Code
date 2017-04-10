using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.LoggingManagement;
using Fmo.Common.Interface;
using Fmo.API.Services.MiddlerWare;
using Fmo.Helpers;
using Fmo.Helpers.Interface;


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

            services.AddMvc();

            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .WithMethods("GET", "PUT", "POST", "DELETE")
                            .AllowAnyHeader();
                    })
            );

            //---Adding scope for all classes
            services.AddSingleton<IExceptionHelper, ExceptionHelper>();
            services.AddSingleton<ILoggingHelper, LoggingHelper>();
            services.AddTransient(_ => new FMODBContext(Configuration.GetConnectionString("FMODBContext")));

            //Infrastructure
            services.AddTransient<IUnitOfWork<FMODBContext>, UnitOfWork<FMODBContext>>();
            services.AddTransient<IDatabaseFactory<FMODBContext>, DatabaseFactory<FMODBContext>>();

            //BusinessServices
            services.AddTransient<IDeliveryPointBusinessService, DeliveryPointBusinessService>();
            services.AddTransient<ISearchBussinessService, SearchBussinessService>();
            services.AddTransient<IAccessLinkBussinessService, AccessLinkBussinessService>();
            services.AddTransient<IAccessLinkRepository, AccessLinkRepository>();
            //Repositories
            services.AddTransient<IDeliveryPointsRepository, DeliveryPointsRepository>();
            services.AddTransient<IPostalAddressBusinessService, PostalAddressBusinessService>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IReferenceDataCategoryRepository, ReferenceDataCategoryRepository>();
            services.AddTransient<IDeliveryRouteBusinessService, DeliveryRouteBusinessService>();
            services.AddTransient<IDeliveryRouteRepository, DeliveryRouteRepository>();
            services.AddTransient<IScenarioRepository, ScenarioRepository>();
            services.AddTransient<IDeliveryUnitLocationRepository, DeliveryUnitLocationRespository>();
            services.AddTransient<IAddressLocationRepository, AddressLocationRepository>();
            services.AddTransient<IPostalAddressRepository, PostalAddressRepository>();
            services.AddTransient<IPostCodeRepository, PostCodeRepository>();
            services.AddTransient<IStreetNetworkRepository, StreetNetworkRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ILoggingHelper, LoggingHelper>();
            services.AddTransient<ICreateOtherLayersObjects, CreateOtherLayerObjects>();
            services.AddTransient<IFileProcessingLogRepository, FileProcessingLogRepository>();
            services.AddTransient<IReferenceDataRepository, ReferenceDataRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=DeliveryPoints}/{action=Get}/{id?}");
            });
            app.UseCors("AllowCors");
        }
    }
}
