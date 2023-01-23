using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL.Decorators;
using ProductionScheduler.Infrastructure.DAL.Repositories;
using ProductionScheduler.Infrastructure.Logging.Decorators;

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
            services.AddScoped<IMachinesRepository, MSSqlMachinesRepository>();

            services.AddScoped<IUnitOfWork, MSSqlUnitOfWork>();
            // chcemy zarezerwować kazdy commandhandler  w calej solucji globalnie , aby jedna operacja żeby za każdym razem
            //kiedy wolany jest dowolny command handler  z 4 , wtedy chcemy zeby wolany byl ten, ktory
            //wstrzykuje unit of work i wykonuje cala operacje w sposob transakcyjny 
            // bylo:  services.TryDecorate<ICommandHandler<ReserveMachineForService>, UnitOfWorkCommandHandlerDecorator>(); 
              services.TryDecorate( typeof(ICommandHandler<>) , typeof(UnitOfWorkCommandHandlerDecorator<>) );
            // czyli kazda ktora jest typu 1 jesdt dekorowana wg typu of 2
            // jego cykl zycia jesdt taki sam jak obiektu dekorowanego  
            //ponizej dodano dodatkowy command handler i tutaj ma kolejnosc znaczenie duze podobno 
          // przeniesiono do logging/extensions.cs services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
            // z tego co rozumiem to idzie od dolu najpeierw logging potem do gory w unit of work zadziala 

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
