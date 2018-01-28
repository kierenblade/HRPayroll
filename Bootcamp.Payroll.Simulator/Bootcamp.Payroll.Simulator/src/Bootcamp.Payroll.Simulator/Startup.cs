using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Bootcamp.Payroll.Simulator.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternate;
using Bootcamp.Payroll.Simulator.Classes;

namespace Bootcamp.Payroll.Simulator
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

            services.AddMvc().AddJsonOptions(opt => {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;  // <<!-- this removes the camelcasing
                }

                // DateTime Serialization/Deserialization Settings
                opt.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AdjustToUniversal });// DateTime will be serialized as UTC
                opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;// DateTime will be converted to Local when deserialized
            });

            services.AddMvc();

            services.Configure<SimAppSettings>(val => Configuration.GetSection("SimSettings").Bind(val));

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
              .AllowAnyHeader()));

            services.Configure<MvcOptions>(options => {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var log = Configuration.GetSection("Logging");

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseCors("AllowAll");
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseMvc();

            var fileSizeLimitBytes = log["FileSizeLimitBytes"];
            long? fileSizeLimitBytesLong = 0;
            if (string.IsNullOrEmpty(fileSizeLimitBytes))
                fileSizeLimitBytesLong = 1024 * 1024;
            else
            {
                fileSizeLimitBytesLong = Convert.ToInt64(fileSizeLimitBytes);
            }

            Log.Logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch((LogEventLevel)Enum.Parse(typeof(LogEventLevel), log["LogLevel"], true)))
            .WriteTo.RollingFileAlternate(
            log["Path"], getLogLevel(log["LogLevel"]), log["OutputTemplate"], fileSizeLimitBytes: fileSizeLimitBytesLong
            ).CreateLogger();

            loggerFactory.AddSerilog();
        }
        private LogEventLevel getLogLevel(string value)
        {
            switch (value)
            {
                case "Debug": return LogEventLevel.Debug;
                case "Error": return LogEventLevel.Error;
                case "Fatal": return LogEventLevel.Fatal;
                case "Information": return LogEventLevel.Information;
                case "Verbose": return LogEventLevel.Verbose;
                case "Warning": return LogEventLevel.Warning;
                default: return LogEventLevel.Debug;
            }
        }
    }
}
