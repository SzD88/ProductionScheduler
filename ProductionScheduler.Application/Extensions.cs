using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Services;

namespace ProductionScheduler.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           //   services.AddScoped<IReservationService, ReservationService>(); // CQRS - deleted while #30 1h:10

            return services;
        }
    }
}
