using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddMSSql(this IServiceCollection services)
        {
            services.AddDbContext<ProductionSchedulerDbContext>(x => x.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ProductionScheduler"));
            return services;

        }
    }
}
