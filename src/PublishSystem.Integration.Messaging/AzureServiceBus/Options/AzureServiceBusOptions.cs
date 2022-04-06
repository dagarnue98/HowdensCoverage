namespace PublishSystem.Integration.Messaging.AzureServiceBus.Options
{
    /// <summary>
    /// AzureServiceBusOptions class to map the appsettings section.
    /// </summary>
    public class AzureServiceBusOptions
    {
        public const string Section = "AzureServiceBus";

        /// <summary>
        /// Gets or sets connectionString.
        /// </summary>
        public string ConnectionString { get; set; }

        public IEnumerable<AzureServiceBusProperties>? Topics { get; set; }
    }
}
