using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL.Decorators;
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
             
            var options = configuration.GetOptions<MSSqlOptions>(SectionName); 
             
            string connectionString = options.ConnectionString;  ;
            services.AddDbContext<ProductionSchedulerDbContext>(x => x.UseSqlServer(connectionString));
            services.AddScoped<IMachinesRepository, MSSqlMachineRepository>();
            services.AddScoped<IUserRepository, MSSqlUserRepository>(); 

            services.AddScoped<IUnitOfWork, MSSqlUnitOfWork>();
           
              services.TryDecorate( typeof(ICommandHandler<>) , typeof(UnitOfWorkCommandHandlerDecorator<>) );
             
            services.AddHostedService<DatabaseInitializer>();
            return services; 
        }  
        public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T: class, new()
        {
            var options = new T();  
            var section = configuration.GetSection(sectionName);
             section.Bind(options);

            return options;
        }
    }
}
