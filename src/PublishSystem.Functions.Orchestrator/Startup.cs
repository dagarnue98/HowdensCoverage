using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishSystem.Application.DependencyInjection;
using PublishSystem.Application.Options;
using PublishSystem.Core.Common.Serializations.Json.DependencyInjection;
using PublishSystem.Domain.DependencyInjections;
using PublishSystem.Domain.Options;
using PublishSystem.Integration.Encoding.DependencyInjection;
using PublishSystem.Integration.Messaging.AzureServiceBus.Options;
using PublishSystem.Integration.Messaging.DependencyInjection;
using PublishSystem.Integration.Notification.Options;
using PublishSystem.Integration.Rendering.DependencyInjection;
using PublishSystem.Orchestrator.Functions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace PublishSystem.Orchestrator.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Options.
            var configuration = builder.GetContext().Configuration;
            var appSettingsOptions = configuration.Get<AppSettingsOptions>();
            var azureSQLDatabaseOptions = configuration.GetSection(AzureSQLDatabaseOptions.Section).Get<AzureSQLDatabaseOptions>();
            var azureServiceBusOptions = configuration.GetSection(AzureServiceBusOptions.Section).Get<AzureServiceBusOptions>();
            var dbConnectionString = azureSQLDatabaseOptions.ConnectionString;
            var notificationOptions = configuration.GetSection(NotificationOptions.Section).Get<NotificationOptions>();
            builder.Services.AddMediaServiceOptions();

            // Core injections.
            builder.Services.AddJsonSerializer();

            // Application layer injections.
            builder.Services.AddAppSettingsOptions();
            builder.Services.AddServices(ServiceLifetime.Scoped);
            builder.Services.AddApplicationInsights();
            builder.Services.AddSerilog(appSettingsOptions.Logs.LogFormat, dbConnectionString, appSettingsOptions.Logs.DBLogTable);

            // Domain layer injections.
            builder.Services.AddDbContext(dbConnectionString, ServiceLifetime.Scoped);
            builder.Services.AddUnitOfWork(ServiceLifetime.Scoped);
            builder.Services.AddRepositories(ServiceLifetime.Scoped);
            builder.Services.AddBusinessRules(ServiceLifetime.Scoped);
            builder.Services.AddMapper(ServiceLifetime.Transient);

            // Integration injections.
            builder.Services.AddMessagingServices(azureServiceBusOptions, ServiceLifetime.Singleton);
            builder.Services.AddEncodingServices(ServiceLifetime.Scoped);
            builder.Services.AddRenderingServices(ServiceLifetime.Scoped);
            builder.Services.AddNotificationServices(notificationOptions, ServiceLifetime.Singleton);

            // Encoding client creation
            builder.Services.ConfigureEncodingServiceClientAsync(configuration).ConfigureAwait(true);
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);
            FunctionsHostBuilderContext hostingContext = builder.GetContext();
            builder.ConfigurationBuilder
                .SetBasePath(hostingContext.ApplicationRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingContext.EnvironmentName}.json", true, true)
                .AddJsonFile("secrets/appsettings.json", true, true)
                .AddEnvironmentVariables();
        }
    }
}
