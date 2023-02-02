using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //   services.AddScoped<IReservationService, ReservationService>(); // CQRS - deleted while #30 1h:10

            //one of posibilites #refactor 
            //  services.AddScoped<ICommandHandler<ReserveMachineForEmployee>, ReserveMachineForEmployeeHandler>(); // ...ectera

            // zbiera wszystko #30 1:19
            var appAssembly = typeof(ICommandHandler<>).Assembly;

            services.Scan(s => s.FromAssemblies(appAssembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()); 


            return services;
        }
    }
}
