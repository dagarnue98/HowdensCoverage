using Microsoft.Azure.Management.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using PublishSystem.Core.Common.DependencyInjection;
using PublishSystem.Integration.Encoding.AzureMediaServices;
using PublishSystem.Integration.Encoding.Options;
using PublishSystem.Integration.Encoding.Services;

namespace PublishSystem.Integration.Encoding.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        private const string TokenType = "Bearer";
        public static IServiceCollection AddEncodingServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IEncoding), typeof(MediaServiceEncoding), lifetime);
            services.AddServiceDescriptor(typeof(IMediaService), typeof(MediaService), lifetime);

            return services;
        }

        public static async Task<IServiceCollection> ConfigureEncodingServiceClientAsync(this IServiceCollection services, IConfiguration configuration)
        {
            var scopes = new[] { $"{configuration["AzureMediaServices:ArmTokenAudience"]}/.default" };
            IConfidentialClientApplication? app = ConfidentialClientApplicationBuilder.Create(configuration["AzureMediaServices:ClientId"])
                .WithClientSecret(configuration["AzureMediaServices:ClientSecret"])
                .WithAuthority(AzureCloudInstance.AzurePublic, configuration["AzureMediaServices:TenantId"])
                .Build();

            var authResult = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync()
                .ConfigureAwait(false);

            ServiceClientCredentials credentials = new TokenCredentials(authResult.AccessToken, TokenType);
            var mediaServiceClient = new AzureMediaServicesClient(new Uri(configuration["AzureMediaServices:ArmEndpoint"]), credentials)
            {
                SubscriptionId = configuration["AzureMediaServices:SubscriptionId"],
                LongRunningOperationRetryTimeout = int.Parse(configuration["AzureMediaServices:LongRunningOperationRetryTimeout"]),
            };

            services.AddScoped(provider => mediaServiceClient);
            return services;
        }

        public static IServiceCollection AddMediaServiceOptions(this IServiceCollection services)
        {
            services.AddSettingOptions<MediaServicesOptions>(MediaServicesOptions.Section);
            return services;
        }
    }
}