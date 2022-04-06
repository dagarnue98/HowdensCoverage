using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PublishSystem.Core.Common.DependencyInjection;
using PublishSystem.Domain.BusinessRules.StateManager;
using PublishSystem.Domain.Repositories;
using PublishSystem.Domain.Repositories.PublishJobRepository;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.DependencyInjections
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.AddAutoMapper(new List<Assembly> { Assembly.GetExecutingAssembly() }, serviceLifetime: lifetime);
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IEntityRepository), typeof(EntityRepository), lifetime);
            services.AddServiceDescriptor(typeof(IEntityRepository<>), typeof(EntityRepository<>), lifetime);
            services.AddServiceDescriptor(typeof(IPublishJobRepository), typeof(PublishJobRepository), lifetime);
            services.AddServiceDescriptor(typeof(IEmailTemplateRepository), typeof(EmailTemplateRepository), lifetime);
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IUnitOfWork), typeof(UnitOfWork), lifetime);
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection AddBusinessRules(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddServiceDescriptor(typeof(IStateManager), typeof(StateManager), lifetime);
            return services;
        }
    }
}
