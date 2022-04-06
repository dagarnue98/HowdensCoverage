using PublishSystem.Core.Common.Options.LogsOptions;
using PublishSystem.Domain.Options;
using PublishSystem.Integration.Messaging.AzureServiceBus.Options;

namespace PublishSystem.Application.Options
{
    public class AppSettingsOptions
    {
        public AzureServiceBusOptions? AzureServiceBus { get; set; }

        public AzureSQLDatabaseOptions? AzureSQLDatabase { get; set; }

        public LogsOptions? Logs { get; set; }
    }
}
