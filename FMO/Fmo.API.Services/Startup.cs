using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.DataServices.Repositories.Interface;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Entities;

namespace Fmo.API.Services
{
    public class Startup
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


            //---Adding scope for all classes
            services.AddScoped(_ => new FMODBContext(Configuration.GetConnectionString("FMODBContext")));
            services.AddTransient<IDeliveryPointBussinessService, DeliveryPointBusinessService>();
            services.AddTransient<ISearchDeliveryPointsRepository, SearchDeliveryPointsRepository>();
            //services.AddSingleton<FMODBContext, FMODBContext>();           
            services.AddTransient<IUnitOfWork<FMODBContext>, UnitOfWork<FMODBContext>>();
            services.AddTransient<IDatabaseFactory<FMODBContext>, DatabaseFactory<FMODBContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=DeliveryPoints}/{action=Get}/{id?}");
            });
        }
    }
}
