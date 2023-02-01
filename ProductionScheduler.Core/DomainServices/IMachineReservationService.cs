using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.DomainServices
{
    public interface IMachineReservationService
    {
        void ReserveMachineForUser(IEnumerable<Machine> allMachineReservations, EmplooyeeRank rank,
            Machine machineToReserve, MachineReservation reservation);

        void ReserveMachineForService(IEnumerable<Machine> allMachines, Date date, Hour hour);

    }
}
