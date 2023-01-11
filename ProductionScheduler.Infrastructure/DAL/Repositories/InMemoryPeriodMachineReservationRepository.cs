using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
    internal class InMemoryPeriodMachineReservationRepository : IPeriodMachineReservationRepository
    {

        private readonly List<PeriodMachineReservation> _periodMachineReservations;


        public InMemoryPeriodMachineReservationRepository(IClock clock)
        {
            _periodMachineReservations = new List<PeriodMachineReservation>()
            {
                new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(clock.Current()), "P1"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(clock.Current()), "P2"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(clock.Current()), "P3"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(clock.Current()), "P4"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(clock.Current()), "P5")
            };
        }
        public Task<PeriodMachineReservation> GetAsync(MachineId id)
        {
            return Task.FromResult(_periodMachineReservations.SingleOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<PeriodMachineReservation>> GetAllAsync()
        {
            return Task.FromResult(_periodMachineReservations.AsEnumerable());
        }
        public Task CreateAsync(PeriodMachineReservation reservation)
        {
            _periodMachineReservations.Add(reservation);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PeriodMachineReservation command)
        {
            return Task.CompletedTask; 
        } 

        public Task DeleteAsync(PeriodMachineReservation command)
        {
            _periodMachineReservations.Remove(command);
            return Task.CompletedTask;
        }


    }
}
