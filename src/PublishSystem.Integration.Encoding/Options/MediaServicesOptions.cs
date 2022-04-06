namespace PublishSystem.Integration.Encoding.Options
{
    public class MediaServicesOptions
    {
        public const string Section = "AzureMediaServices";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantDomain { get; set; }
        public string TenantId { get; set; }
        public string MediaServicesAccountName { get; set; }
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }
        public string ArmTokenAudience { get; set; }
        public string ArmEndpoint { get; set; }
        public string TransformName { get; set; }
        public string AuthRedirectUri { get; set; }
        public string AuthClientApplicationId { get; set; }
        public string DefaultStreamingEndpointName { get; set; }
        public int JobCheckSeconds { get; set; }
        public int LongRunningOperationRetryTimeout { get; set; }
    }
}
