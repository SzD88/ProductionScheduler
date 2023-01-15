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
        => true;
    }
}
