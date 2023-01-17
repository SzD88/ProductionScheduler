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
                var expectedMachinesTable = new List<Machine>()
                    {

                       new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(clock.Current()), "P1"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(clock.Current()), "P2"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(clock.Current()), "P3"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(clock.Current()), "P4"),
                       new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(clock.Current()), "P5")
                    };

                var machinesToReserve = dbContext.PeriodMachineReservations.ToList();

                var firstMachine = machinesToReserve.FirstOrDefault();
                bool clearTable = false;
                if (firstMachine is not null)
                {
                      clearTable = firstMachine.TimeForward == expectedMachinesTable.FirstOrDefault().TimeForward;

                }

                if (!machinesToReserve.Any() || machinesToReserve.Count < expectedMachinesTable.Count || clearTable) // #refactor - shouldnt it check if number of machines is 
                                                                                                                       //equal to 5 or new created sum of machines?
                {
                    //  var clock = new Clock();

                    foreach (var item in machinesToReserve)
                    {
                        dbContext.Remove(item);
                    }

                    machinesToReserve = expectedMachinesTable;

                    dbContext.AddRange(machinesToReserve);
                    dbContext.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
        public async Task ClearTimeForwardAsync(ProductionSchedulerDbContext dbContext)
        {
            var machines = await dbContext.PeriodMachineReservations.ToListAsync();

            foreach (var item in machines)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges(); 
        }
    }
}
