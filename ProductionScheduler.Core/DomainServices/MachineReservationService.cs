using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Policies;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.DomainServices
{
    internal sealed class MachineReservationService : IMachineReservationService
    {
        private readonly IEnumerable<IReservationPolicy> _policies;
        private readonly IClock _clock;

        public MachineReservationService(IEnumerable<IReservationPolicy> policies, IClock clock)
        {
            _policies = policies;
            _clock = clock;
        }
        public void ReserveMachineForService(IEnumerable<Machine> allMachines, Date date, Hour hour)
        {
            foreach (var machine in allMachines)
            {
                var reservationsForSameDate = machine.Reservations
                    .Where(x => x.Date == date)
                    .Where(y => y.Hour == hour);

                machine.RemoveReservations(reservationsForSameDate);

                var serviceReservation = new ReservationForService(ReservationId.Create(), machine.Id, date, hour);
                machine.AddReservation(serviceReservation, new Date(_clock.Current()));
            }
        }
        public void ReserveMachineForUser(IEnumerable<Machine> allMachineReservations,
            EmplooyeeRank rank, Machine machine, ReservationForUser reservation)
        {
            var machineToReserveId = machine.Id;
            var policy = _policies.SingleOrDefault(x => x.CanBeApplied(rank));
            if (policy is null)
            {
                throw new NoReservationPolicyFoundException(rank);
            }
            if (policy.CanReserve(allMachineReservations, reservation.EmployeeName) is false)
            {
                throw new CannotReserveMachineException(machineToReserveId);
            } 
            machine.AddReservation(reservation, new Date(_clock.Current()));
        }
    }
}
