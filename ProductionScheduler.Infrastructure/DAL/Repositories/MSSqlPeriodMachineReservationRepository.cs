using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
    internal class MSSqlPeriodMachineReservationRepository : IPeriodMachineReservationRepository
    {

        private readonly ProductionSchedulerDbContext _dbContextReservations;
        public MSSqlPeriodMachineReservationRepository(ProductionSchedulerDbContext dbContext)
        {
            _dbContextReservations = dbContext;
        }
        public Task<PeriodMachineReservation> GetAsync(MachineId id)
        {
            return _dbContextReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        async Task<IEnumerable<PeriodMachineReservation>> GetByPeriodAsync(ReservationTimeForward timeForward)
        {
            return await _dbContextReservations.PeriodMachineReservations
                       .Include(x => x.Reservations) // eager loading a nie lazy loading 
                       .Where(x => x.TimeForward == timeForward)
                       .ToListAsync();
        }
        public async Task<IEnumerable<PeriodMachineReservation>> GetAllAsync()
        {
            var result = await _dbContextReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .ToListAsync();
            return result.AsEnumerable();
        }


        public async Task CreateAsync(PeriodMachineReservation command)
        {
            await _dbContextReservations.AddAsync(command);
            await _dbContextReservations.SaveChangesAsync();
        }
        public async Task UpdateAsync(PeriodMachineReservation command)
        {
            _dbContextReservations.Update(command);
            await _dbContextReservations.SaveChangesAsync();
        }
        public async Task DeleteAsync(PeriodMachineReservation command)
        {
            _dbContextReservations.Remove(command);
            await _dbContextReservations.SaveChangesAsync();
        }


    }
}
