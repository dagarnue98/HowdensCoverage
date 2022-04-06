namespace PublishSystem.Integration.Messaging.AzureServiceBus.Options
{
    /// <summary>
    /// AzureServiceBusProperties class to map the appsettings section.
    /// </summary>
    public class AzureServiceBusProperties
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets connectionString.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets entityPath (topic name or queue name).
        /// </summary>
        public string? EntityPath { get; set; }
    }
}
