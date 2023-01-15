using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Policies;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.DomainServices
{
    internal sealed class MachineReservationService : IMachineReservationService
    {
        // tu przypisano wwzystkie 3 polityki #refactor usun
        private readonly IEnumerable<IReservationPolicy> _policies;
        private readonly IClock _clock;

        public MachineReservationService(IEnumerable<IReservationPolicy> policies, IClock clock)
        {
            _policies = policies;
            _clock = clock;
        }
        // wzorzec #refactor wzorzec strategii - if do pojedynczej klasy #tu koniec 16 min

        public void ReserveMachineForUser(IEnumerable<PeriodMachineReservation> allMachineReservations,
            EmplooyeeRank rank, PeriodMachineReservation machineToReserve, Reservation reservation)
        {
            var machineToReserveId = machineToReserve.Id;
            // przypisz te 1 polityke ktora zwroci true bo rank == jobtitle #refactor 
            var policy = _policies.SingleOrDefault(x => x.CanBeApplied(rank));
            if (policy is null)
            {
                throw new NoReservationPolicyFoundException(rank);
            }
            if (policy.CanReserve(allMachineReservations, reservation.EmployeeName) is false)
            {
                throw new CannotReserveMachineException(machineToReserveId);
            }

            machineToReserve.AddReservation(reservation, new Date( _clock.Current()));
        }
    }
}
