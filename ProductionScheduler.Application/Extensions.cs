using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        { 
            var appAssembly = typeof(ICommandHandler<>).Assembly;

            services.Scan(s => s.FromAssemblies(appAssembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()); 
             
            return services;
        }
    }
}
