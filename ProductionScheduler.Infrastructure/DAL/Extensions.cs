using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL.Repositories;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddMSSql(this IServiceCollection services)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ProductionScheduler";
            services.AddDbContext<ProductionSchedulerDbContext>(x => x.UseSqlServer(connectionString));
            services.AddScoped<IPeriodMachineReservationRepository, MSSqlPeriodMachineReservationRepository>();
            services.AddHostedService<DatabaseInitializer>();
            return services;

        }
    }
}
