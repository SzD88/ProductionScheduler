 
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Infrastructure.Repositories
{
    internal class InMemoryPeriodMachineReservationRepository : IPeriodMachineReservationRepository
    {
        private readonly IClock _clock;

        private readonly List<PeriodMachineReservation> _periodMachineReservations;


        public InMemoryPeriodMachineReservationRepository(IClock clock)
        {
            _clock = clock;
            _periodMachineReservations = new List<PeriodMachineReservation>()
            {
                new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(clock.Current()), "P1"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(clock.Current()), "P2"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(clock.Current()), "P3"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(clock.Current()), "P4"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(clock.Current()), "P5")
            };
        }
        public PeriodMachineReservation Get(MachineId id)
        {
            return _periodMachineReservations.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<PeriodMachineReservation> GetAll()
        {
            return _periodMachineReservations;
        }

        public void Update(PeriodMachineReservation command)
        {
            // nothing
        }

        public void Create(PeriodMachineReservation command)
        {
            _periodMachineReservations.Add(command);
        }

        public void Delete(PeriodMachineReservation command)
        {
            _periodMachineReservations.Remove(command);
        }


    }
}
