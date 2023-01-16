using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure.DAL;
using ProductionScheduler.Infrastructure.Exceptions;

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

         //   services.AddSingleton<IClock, Clock>();
           //  services.AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>();
             services.AddMSSql(configuration)
                .AddSingleton<IClock,Clock>();
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app) { 
        

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers(); // could be hidden here #refactor
            return app;
        }
    }
}
