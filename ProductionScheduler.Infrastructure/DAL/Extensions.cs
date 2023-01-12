using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL.Repositories;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal static class Extensions
    {
        private const string SectionName = "database";
        public static IServiceCollection AddMSSql(this IServiceCollection services, IConfiguration configuration)
        { 
            var section = configuration.GetSection(SectionName);
            services.Configure<MSSqlOptions>(section);

            //var options = new MSSqlOptions(); // to the bottom
            //section.Bind(options);
            //step 2 : 
            // var options = GetOptions<MSSqlOptions>(configuration, "dastabase");
            var options = configuration.GetOptions<MSSqlOptions>(SectionName); //GetOptions<MSSqlOptions>(configuration,"database"); // was
            
            
            string connectionString = options.ConnectionString;
            //   string connectionString = configuration["database:connectionstring"]  ;
            services.AddDbContext<ProductionSchedulerDbContext>(x => x.UseSqlServer(connectionString));
            services.AddScoped<IPeriodMachineReservationRepository, MSSqlPeriodMachineReservationRepository>();
            services.AddHostedService<DatabaseInitializer>();
            return services;

        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T: class, new()
        {
            var options = new T(); // to the bottom
            var section = configuration.GetSection(sectionName);
             section.Bind(options);

            return options;
        }
    }
}
