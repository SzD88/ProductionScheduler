using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure.DAL;
using ProductionScheduler.Infrastructure.Exceptions;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Infrastructure.Logging;
using ProductionScheduler.Infrastructure.Security;

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

            services.AddSingleton<ExceptionMiddleware>();

            services.AddSecurity();

         //   services.AddSingleton<IClock, Clock>();
           //  services.AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>();
             services.AddMSSql(configuration)
                .AddSingleton<IClock,Clock>();

            services.AddCustomLogging(); // po mssql - bo dopiero po bazie danych ma byc uzyte 

            var infrastructureAssembly = typeof(AppOptions).Assembly;

            services.Scan(s => s.FromAssemblies(infrastructureAssembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app) { 
        

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers(); // could be hidden here #refactor
            return app;
        }
    }
}
