using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.Entities;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.DataManagement.PostalAddress.WebAPI
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

            LogWriterFactory log = new LogWriterFactory();
            LogWriter logWriter = log.Create();
            Logger.SetLogWriter(logWriter, false);

            //---Adding scope for all classes
            services.AddSingleton<ILoggingHelper, LoggingHelper>(serviceProvider =>
            {
                return new LoggingHelper(logWriter);
            });

            services.AddSingleton<IExceptionHelper, ExceptionHelper>(serviceProvider =>
            {
                return new ExceptionHelper(logWriter);
            });

            // Register Infrastructure
            services.AddScoped<IDatabaseFactory<PostalAddressDBContext>, DatabaseFactory<PostalAddressDBContext>>();

            // Register DataServices
            services.AddScoped<IPostalAddressDataService, PostalAddressDataService>();
            services.AddScoped<IFileProcessingLogDataService, FileProcessingLogDataService>();

            // Register Integration Services
            services.AddScoped<IPostalAddressIntegrationService, PostalAddressIntegrationService>();

            // Register Business Service
            services.AddScoped<IPostalAddressBusinessService, PostalAddressBusinessService>();

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