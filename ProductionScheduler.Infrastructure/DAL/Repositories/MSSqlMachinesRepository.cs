using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
    internal class MSSqlMachineRepository : IMachinesRepository
    { 
        private readonly ProductionSchedulerDbContext _dbContextReservations;
        public MSSqlMachineRepository(ProductionSchedulerDbContext dbContext)
        {
            _dbContextReservations = dbContext;
        }
        public Task<Machine> GetAsync(MachineId id)
        {
            return _dbContextReservations.Machines
                .Include(x => x.Reservations)  
                .SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Machine>> GetByPeriodAsync(ReservationTimeForward timeForward)
        {
            return await _dbContextReservations.Machines  
                       .Include(x => x.Reservations)  
                       .Where(x => x.TimeForward == timeForward)
                       .ToListAsync();
        } 
        public async Task<IEnumerable<Machine>> GetAllAsync()
        {
            var result = await _dbContextReservations.Machines
                .Include(x => x.Reservations)  
                .ToListAsync();
            return result.AsEnumerable();
        } 
        public async Task CreateAsync(Machine command)
        {
            await _dbContextReservations.AddAsync(command); 
        }
        public   Task UpdateAsync(Machine command)
        {
             _dbContextReservations.Update(command); 
           return Task.CompletedTask;
        }
        public   Task DeleteAsync(Machine command)
        {
            _dbContextReservations.Remove(command); 
            return Task.CompletedTask; 
        } 
    }
}
