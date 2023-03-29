using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.DomainServices
{
    public interface IMachineService
    {
        void ReserveMachineForUser(IEnumerable<Machine> allMachineReservations, EmplooyeeRank rank,
            Machine machineToReserve, ReservationForUser reservation);
        void ReserveMachineForService(IEnumerable<Machine> allMachines, Date date, Hour hour);

    }
}
