using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Core.DomainServices;
using ProductionScheduler.Core.Policies;

namespace ProductionScheduler.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {

            services.AddSingleton<IReservationPolicy, EmployeeReservationPolicy>();
            services.AddSingleton<IReservationPolicy, ManagerReservationPolicy>();
            services.AddSingleton<IReservationPolicy, AdminReservationPolicy>();
            services.AddSingleton<IMachineReservationService, MachineReservationService>();
            return services;
        }
    }
}
