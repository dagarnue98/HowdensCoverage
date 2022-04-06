using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PublishSystem.Core.Common.Serializations.Json.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddJsonSerializer(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializer, TextJsonSerializer>();
            services.TryAddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return services;
        }

        public static IServiceCollection AddJsonSerializer(this IServiceCollection services, JsonSerializerOptions settings)
        {
            services.AddSingleton<IJsonSerializer, TextJsonSerializer>();
            services.TryAddSingleton(settings);
            return services;
        }
    }
}
