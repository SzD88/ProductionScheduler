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
        public bool CanReserve(IEnumerable<Machine> machines, EmployeeName name) 
        {
            var totalEmployeeReservations = machines
                .SelectMany(x => x.Reservations)
                .OfType<ReservationForUser>()
                .Count(x => x.EmployeeName == name);
 
            var clockDay = _clock.Current().Day;
            var clockhay = _clock.Current().Hour;
            var daysForwardUserCanReserve = 2;

            var toWhenEmployeeCanReserve = DateTime.UtcNow.AddDays(daysForwardUserCanReserve);
             
            var answ = totalEmployeeReservations < 2; 
            return answ;
        }
    }
}
