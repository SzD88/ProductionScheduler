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

        public bool CanReserve(IEnumerable<Machine> machines, EmployeeName name) //#refactor zmienic na user id zamiast name  ?  reservationDate ? jezeli chcesz pozwolic
            // na rezerwacje tylko danego dnia  ale moze nie komplikowac juz ? 
        {
            var totalEmployeeReservations = machines
                .SelectMany(x => x.Reservations)
                .OfType<ReservationForUser>()
                .Count(x => x.EmployeeName == name);

            // #refactor  // w zaalozeniu pracownik regularny moze rezerwowac tylko wolna maszyne tyko 2 godziny do przodu
            var clockDay = _clock.Current().Day;
            var clockhay = _clock.Current().Hour;
            var daysForwardUserCanReserve = 2;

            var toWhenEmployeeCanReserve = DateTime.UtcNow.AddDays(daysForwardUserCanReserve);

            // pracownik moze rezerwowac tylko dzisiaj #refactor
            var answ = totalEmployeeReservations < 12;  // && _clock.Current().Day == DateTime.UtcNow.Day;
            return answ;
        }
    }
}
