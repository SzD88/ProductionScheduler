using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal sealed class DatabaseInitializer : IHostedService // BackgroundService, - operations in background while true -> do smth every 5 min for example
                                                               //background operations #21
    {
        private readonly IServiceProvider _serviceProvider;
        public DatabaseInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken) // on app start
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductionSchedulerDbContext>();
                dbContext.Database.Migrate();


                var clock = new Clock();
                var expectedMachinesTable = new List<MachineToReserve>()
                    {

                       new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(clock.Current()), "P1"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(clock.Current()), "P2"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(clock.Current()), "P3"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(clock.Current()), "P4"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(clock.Current()), "P5")
                    };


                var periodMachineReservations = dbContext.PeriodMachineReservations.ToList();

                if (!periodMachineReservations.Any() || periodMachineReservations.Count < expectedMachinesTable.Count) // #refactor - shouldnt it check if number of machines is 
                                                                                                                       //equal to 5 or new created sum of machines?
                {
                    //  var clock = new Clock();

                    foreach (var item in periodMachineReservations)
                    {
                        dbContext.Remove(item);
                    }

                    periodMachineReservations = expectedMachinesTable;

                    dbContext.AddRange(periodMachineReservations);
                    dbContext.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
