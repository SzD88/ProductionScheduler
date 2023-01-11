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

        private readonly ProductionSchedulerDbContext _periodMachineReservations;
        public MSSqlPeriodMachineReservationRepository(ProductionSchedulerDbContext dbContext)
        {
            _periodMachineReservations = dbContext;
        }
        public Task<PeriodMachineReservation> GetAsync(MachineId id)
        {
            return _periodMachineReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PeriodMachineReservation>> GetAllAsync()
        {
            var result = await _periodMachineReservations.PeriodMachineReservations
                .Include(x => x.Reservations) // eager loading a nie lazy loading 
                .ToListAsync();
            return result.AsEnumerable();
        }


        public async Task CreateAsync(PeriodMachineReservation command)
        {
            await _periodMachineReservations.AddAsync(command);
            await _periodMachineReservations.SaveChangesAsync();
        }
        public async Task UpdateAsync(PeriodMachineReservation command)
        {
            _periodMachineReservations.Update(command);
            await _periodMachineReservations.SaveChangesAsync();
        }
        public async Task DeleteAsync(PeriodMachineReservation command)
        {
            _periodMachineReservations.Remove(command);
            await _periodMachineReservations.SaveChangesAsync();
        }

        
    }
}
