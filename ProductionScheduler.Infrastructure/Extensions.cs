using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure.Auth;
using ProductionScheduler.Infrastructure.DAL;
using ProductionScheduler.Infrastructure.Exceptions;
using ProductionScheduler.Infrastructure.Logging;
using ProductionScheduler.Infrastructure.Security;
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
            services.Configure<AppOptions>(section);

            services.AddSingleton<ExceptionMiddleware>();

            services.AddSecurity();
            services.AddAuth(configuration);
            services.AddHttpContextAccessor();
            services.AddMSSql(configuration)
               .AddSingleton<IClock, Clock>();

            var infrastructureAssembly = typeof(AppOptions).Assembly;

            services.Scan(s => s.FromAssemblies(infrastructureAssembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddCustomLogging();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Production Sheduler API",
                    Version = "v1"
                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", 
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                swagger.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });
            return services;
        } 
        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();
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
