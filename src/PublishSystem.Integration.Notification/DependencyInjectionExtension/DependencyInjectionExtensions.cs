using PublishSystem.Core.Common.DependencyInjection;
using PublishSystem.Integration.Notification;
using PublishSystem.Integration.Notification.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, NotificationOptions notificationOptions, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddNotificationClient(notificationOptions, lifetime);
            return services;
        }

        private static void AddNotificationClient(this IServiceCollection services, NotificationOptions notificationOptions, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddServiceDescriptor(typeof(IEmailSender), provider =>
            {
                var sender = new EmailSender(notificationOptions.IntegrationUrl);
                return sender;

            }, lifetime);
        }
    }
}
