﻿using Fmo.BusinessServices.Interfaces;
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
            services.AddScoped(_ => new FMODBContext(Configuration.GetConnectionString("FMODBContext")));
            services.AddTransient<IDeliveryPointBusinessService, DeliveryPointBusinessService>();
            services.AddTransient<IDeliveryPointsRepository, DeliveryPointsRepository>();    
            services.AddTransient<IUnitOfWork<FMODBContext>, UnitOfWork<FMODBContext>>();
            services.AddTransient<IDatabaseFactory<FMODBContext>, DatabaseFactory<FMODBContext>>();
            services.AddTransient<IPostalAddressBusinessService, PostalAddressBusinessService>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IReferenceDataCategoryRepository, ReferenceDataCategoryRepository>();
            services.AddTransient<IDeliveryRouteBusinessService, DeliveryRouteBusinessService>();
            services.AddTransient<IDeliveryRouteRepository, DeliveryRouteRepository>();
            services.AddTransient<IScenarioRepository, ScenarioRepository>();
            services.AddTransient<IDeliveryUnitLocationRepository, DeliveryUnitLocationRespository>();
            services.AddTransient<IAddressLocationRepository, AddressLocationRepository>();
            services.AddTransient<IPostalAddressRepository, PostalAddressRepository>();
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
