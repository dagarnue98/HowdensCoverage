using Microsoft.Extensions.DependencyInjection;
using PublishSystem.Core.Common.DependencyInjection;

namespace PublishSystem.Integration.Rendering.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRenderingServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IRendering), typeof(Rendering), lifetime);
            return services;
        }
    }
}
