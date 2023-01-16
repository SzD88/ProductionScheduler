using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.DomainServices
{
    public interface IMachineReservationService
    {
        void ReserveMachineForUser(IEnumerable<MachineToReserve> allMachineReservations, EmplooyeeRank rank,
            MachineToReserve machineToReserve, MachineReservation reservation);

        void ReserveMachineForService(IEnumerable<MachineToReserve> allMachines, Date date, Hour hour);

    }
}
