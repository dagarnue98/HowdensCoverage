using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishSystem.Application.Options;
using PublishSystem.Application.Services.EmailService;
using PublishSystem.Application.Services.PublishJobService;
using PublishSystem.Application.Services.RenderingService;
using PublishSystem.Core.Common.DependencyInjection;
using PublishSystem.Domain.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace PublishSystem.Application.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IPublishJobService), typeof(PublishJobService), lifetime);
            services.AddServiceDescriptor(typeof(IRenderingService), typeof(RenderingService), lifetime);
            services.AddServiceDescriptor(typeof(IEmailService), typeof(EmailService), lifetime);
            return services;
        }

        public static IServiceCollection AddAppSettingsOptions(this IServiceCollection services)
        {
            services.AddOptions<AzureSQLDatabaseOptions>().Configure<IConfiguration>((settings, config) => config.GetSection(AzureSQLDatabaseOptions.Section).Bind(settings));
            services.AddOptions<AppSettingsOptions>().Configure<IConfiguration>((settings, config) => config.Bind(settings));
            return services;
        }

        public static IServiceCollection AddSerilog(this IServiceCollection services, string logOutputTemplate, string dbConnectionString, string DBTableName)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: logOutputTemplate)
                .WriteTo.ApplicationInsights(
                    TelemetryConfiguration.CreateDefault(),
                    TelemetryConverter.Traces,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.MSSqlServer(
                    connectionString: dbConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = DBTableName },
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            services.AddLogging(log => { log.AddSerilog(Log.Logger, true); });
            return services;
        }

        public static IServiceCollection AddApplicationInsights(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            return services;
        }
    }
}
