using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
    internal class MSSqlPeriodMachineReservationRepository : IPeriodMachineReservationRepository
    {

        private readonly ProductionSchedulerDbContext _dbContextReservations;
        public MSSqlPeriodMachineReservationRepository(ProductionSchedulerDbContext dbContext)
        {
            _dbContextReservations = dbContext;
        }
        public Task<Machine> GetAsync(MachineId id)
        {
            return _dbContextReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Machine>> GetByPeriodAsync(ReservationTimeForward timeForward)
        {
            return await _dbContextReservations.PeriodMachineReservations // zwroci taki period machine reservation,
                                                                          // //ktore jest zawarte w rezerwacjach - wsrod ktorych
                                                                          // time forward = time forward 
                       .Include(x => x.Reservations) // eager loading a nie lazy loading  // powiazanie do innej tabeli - reservations 
                       .Where(x => x.TimeForward == timeForward)
                       .ToListAsync();
        }
        //async Task<IEnumerable<PeriodMachineReservation>> GetByPeriodAsync(ReservationTimeForward timeForward)
        //{
        //    return await _dbContextReservations.PeriodMachineReservations
        //               .Include(x => x.Reservations) // eager loading a nie lazy loading 
        //               .Where(x => x.TimeForward == timeForward)
        //               .ToListAsync();
        //}
        public async Task<IEnumerable<Machine>> GetAllAsync()
        {
            var result = await _dbContextReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .ToListAsync();
            return result.AsEnumerable();
        }


        public async Task CreateAsync(Machine command)
        {
            await _dbContextReservations.AddAsync(command);
            await _dbContextReservations.SaveChangesAsync();
        }
        public async Task UpdateAsync(Machine command)
        {
            _dbContextReservations.Update(command);
            await _dbContextReservations.SaveChangesAsync();
        }
        public async Task DeleteAsync(Machine command)
        {
            _dbContextReservations.Remove(command);
            await _dbContextReservations.SaveChangesAsync();
        }


    }
}
