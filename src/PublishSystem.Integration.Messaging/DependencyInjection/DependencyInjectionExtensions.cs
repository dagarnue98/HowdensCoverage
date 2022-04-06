using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishSystem.Integration.Messaging.AzureServiceBus;
using PublishSystem.Integration.Messaging.AzureServiceBus.Options;

namespace PublishSystem.Integration.Messaging.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddMessagingServices(this IServiceCollection services, AzureServiceBusOptions azureServiceBusOptions, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddAzureServiceBusOptions();
            services.AddAzureServiceBusClient(azureServiceBusOptions.ConnectionString);
            services.AddAzureServiceBusTopics(azureServiceBusOptions, lifetime);
            services.AddEventBus(lifetime);
        }

        private static void AddAzureServiceBusOptions(this IServiceCollection services)
        {
            services.AddOptions<AzureServiceBusOptions>().Configure<IConfiguration>((settings, config) => config.GetSection(AzureServiceBusOptions.Section).Bind(settings));
        }

        private static void AddEventBus(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.Add(new ServiceDescriptor(typeof(IEventBus), typeof(ServiceBusClientManager), lifetime));
        }

        private static void AddAzureServiceBusClient(this IServiceCollection services, string connectionString)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(connectionString);
            });
        }

        private static void AddAzureServiceBusTopics(this IServiceCollection services, AzureServiceBusOptions azureServiceBusOptions, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            foreach (var topic in azureServiceBusOptions.Topics.Distinct())
            {
                services.Add(new ServiceDescriptor(
                    typeof(ISender),
                    provider =>
                    {
                        var serviceBusClient = new ServiceBusClient(topic.ConnectionString);
                        var sender = new Sender(serviceBusClient, topic.EntityPath);
                        return sender;
                    },
                    lifetime));
            }
        }
    }
}