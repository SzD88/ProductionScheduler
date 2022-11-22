using MachineReservations.Api.Entities;
using MachineReservations.Api.Services;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineReservations.Repositories
{
    public class InMemoryPeriodMachineReservationRepository : IPeriodMachineReservationRepository
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
