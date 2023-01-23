using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Infrastructure.Logging.Decorators;
using Serilog;

namespace ProductionScheduler.Infrastructure.Logging
{
    public static class Extensions
    {
        internal static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

            return services;
        }

        public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
        {

            builder.Host.UseSerilog((context, config) =>
                    {
                        config.WriteTo
                            .Console()
                           .WriteTo
                           .File("logs/logs.txt")
                           .WriteTo
                           .Seq("http://localhost:5341"); } );

            return builder;
        }
    }
}
