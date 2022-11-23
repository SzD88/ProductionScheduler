using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.Repositories;

namespace ProductionScheduler.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
              services.AddSingleton<IClock, Clock>();
              services.AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>();
            return services;
        }
    }
}
