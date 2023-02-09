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
using ProductionScheduler.Infrastructure.Auth;

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
            services.AddAuth(configuration);
            services.AddHttpContextAccessor(); //#34 / 55 min

            //   services.AddSingleton<IClock, Clock>();
            //  services.AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>();
            services.AddMSSql(configuration)
               .AddSingleton<IClock, Clock>();


            var infrastructureAssembly = typeof(AppOptions).Assembly;

            services.Scan(s => s.FromAssemblies(infrastructureAssembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddCustomLogging(); // po mssql - bo dopiero po bazie danych ma byc uzyte 
            services.AddEndpointsApiExplorer(); //refactor #refactor do minimal api

            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Production Sheduler API",
                    Version = "v1"
                });

            });

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {


            app.UseMiddleware<ExceptionMiddleware>();


            app.UseSwagger();
            //  app.UseSwaggerUI();
            app.UseReDoc(reDoc =>
            {
                reDoc.RoutePrefix = "docs";
                reDoc.DocumentTitle = "Production Sheduler API";
                reDoc.SpecUrl = ("/swagger/v1/swagger.json");

            }
            ); ;

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
