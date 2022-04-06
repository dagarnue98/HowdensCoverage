using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PublishSystem.Core.Common.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServiceDescriptor(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
            return services;
        }

        public static IServiceCollection AddSettingOptions<T>(this IServiceCollection services, string section) where T : class
        {
            services.AddOptions<T>().Configure<IConfiguration>((settings, config) => config.GetSection(section).Bind(settings));
            return services;
        }

        public static IServiceCollection AddServiceDescriptor(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(serviceType, lifetime));
            return services;
        }

        public static IServiceCollection AddServiceDescriptor(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
            return services;
        }
    }
}
