using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Policies
{
    internal sealed class AdminReservationPolicy : IReservationPolicy
    {
        private readonly IClock _clock;

        public AdminReservationPolicy(IClock clock)
        {
            _clock = clock;
        }

        public bool CanBeApplied(EmplooyeeRank rank)
        {
            return rank == EmplooyeeRank.Admin;

        }

        public bool CanReserve(IEnumerable<PeriodMachineReservation> periodMachineReservations, EmployeeName name)
        {
            return true;
        }
    }
}
