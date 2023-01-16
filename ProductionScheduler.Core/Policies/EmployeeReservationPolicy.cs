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

        public bool CanReserve(IEnumerable<MachineToReserve> periodMachineReservations, EmployeeName name)
        {
            var totalEmployeeReservations = periodMachineReservations.SelectMany(x => x.Reservations)
                .Count(x => x.EmployeeName == name);

            // #refactor  // w zaalozeniu pracownik regularny moze rezerwowac tylko wolna maszyne tyko 2 godziny do przodu
            var clockDay = _clock.Current().Day;
            var clockhay = _clock.Current().Hour;

            // pracownik moze rezerwowac tylko dzisiaj #refactor
            var answ = totalEmployeeReservations < 2 && _clock.Current().Day == DateTime.UtcNow.Day;
            return answ;
        }
    }
}
