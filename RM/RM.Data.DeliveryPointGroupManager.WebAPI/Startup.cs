﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Data.DeliveryPointGroupManager.WebAPI
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
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

            services.AddApplicationInsightsTelemetry(Configuration);

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

            //  services.AddScoped<IDatabaseFactory<RouteDBContext>, DatabaseFactory<RouteDBContext>>();
            //   services.AddScoped<IHttpHandler, HttpHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("AllowCors");

            ConfigureAuth(app);
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            MapExceptionTypes(app);

            app.UseMvc();

            WebHelpers.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }
    }
}