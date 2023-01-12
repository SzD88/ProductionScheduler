using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL;
using ProductionScheduler.Infrastructure.DAL.Repositories;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MachineReservations.Tests.Unit")]
namespace ProductionScheduler.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure
            (this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection("app");
            services.Configure<AppOptions>(section); //Microsoft.Extensions.Configuration;
            services.AddSingleton<IClock, Clock>();
           //  services.AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>();
             services.AddMSSql(configuration);
            return services;
        }
    }
}
