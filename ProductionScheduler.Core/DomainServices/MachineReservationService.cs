using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.DomainServices
{
    public sealed class MachineReservationService : IMachineReservationService
    {
        public void ReserveMachineForUser(IEnumerable<PeriodMachineReservation> allMachineReservations, 
            EmplooyeeRank rank, PeriodMachineReservation machineToReserve, Reservation reservation)
        {
           // wzorzec #refactor wzorzec strategii - if do pojedynczej klasy #tu koniec 16 min
        }
    }
}
