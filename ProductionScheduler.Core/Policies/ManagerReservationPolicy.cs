using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Policies
{
    internal sealed class ManagerReservationPolicy : IReservationPolicy
    {
        private readonly IClock _clock;

        public ManagerReservationPolicy(IClock clock)
        {
            _clock = clock;
        }

        public bool CanBeApplied(EmplooyeeRank rank)
        {
            return rank == EmplooyeeRank.Manager;
        }

        public bool CanReserve(IEnumerable<PeriodMachineReservation> periodMachineReservations, EmployeeName name)
        {
            var totalEmployeeReservations = periodMachineReservations.SelectMany(x => x.Reservations)
                   .Count(x => x.EmployeeName == name);

            // #refactor  // w zaalozeniu pracownik regularny moze rezerwowac tylko wolna maszyne tyko 2 godziny do przodu
            return totalEmployeeReservations < 5 && _clock.Current().Day > DateTime.UtcNow.Day + 2;
        }
    }
}
