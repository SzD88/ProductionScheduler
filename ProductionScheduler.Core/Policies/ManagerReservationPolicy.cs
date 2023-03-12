using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Policies
{
    internal sealed class ManagerReservationPolicy : IReservationPolicy
    {
        private readonly IClock _clock;
        private readonly int numbersOfPossibleReservations = 5;
        private readonly int daysForwardForReservations = 2;
        public ManagerReservationPolicy(IClock clock)
        {
            _clock = clock;
        } 
        public bool CanBeApplied(EmplooyeeRank rank)
        {
            return rank == EmplooyeeRank.Manager;
        }

        public bool CanReserve(IEnumerable<Machine> periodMachineReservations, EmployeeName name)
        {
            var totalEmployeeReservations = periodMachineReservations
                .SelectMany(x => x.Reservations)
                .OfType<ReservationForUser>()
                   .Count(x => x.EmployeeName == name); 

            return totalEmployeeReservations < numbersOfPossibleReservations && _clock.Current().Day > DateTime.UtcNow.Day + daysForwardForReservations;
        }
    }
}
