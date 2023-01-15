using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Policies
{
    internal sealed class EmployeeReservationPolicy : IReservationPolicy
    {
        private readonly IClock _clock;
        public EmployeeReservationPolicy(IClock clock)
        {
            _clock = clock;
        }
        public bool CanBeApplied(EmplooyeeRank rank)
        {
            return rank == EmplooyeeRank.Employee;
        }

        public bool CanReserve(IEnumerable<PeriodMachineReservation> periodMachineReservations, EmployeeName name)
        {
            var totalEmployeeReservations = periodMachineReservations.SelectMany(x => x.Reservations)
                .Count(x => x.EmployeeName == name);

            // #refactor  // w zaalozeniu pracownik regularny moze rezerwowac tylko wolna maszyne tyko 2 godziny do przodu
            return totalEmployeeReservations < 2 && _clock.Current().Hour > DateTime.UtcNow.Hour + 2;
              
        }
    }
}
