using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure;
using Serilog;
using WebApi.Installers;

namespace WebAPI.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMemoryCache()
                .AddSingleton<IClock, Clock>()
                .AddCore()
                .AddApplication()
                .AddInfrastructure(configuration)
                .AddControllers();
        }
    }
}