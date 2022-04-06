namespace PublishSystem.Domain.Options
{
    public class AzureSQLDatabaseOptions
    {
        public const string Section = "AzureSQLDatabase";

        /// <summary>
        /// Gets or sets connectionString.
        /// </summary>
        public string? ConnectionString { get; set; }
    }
}
