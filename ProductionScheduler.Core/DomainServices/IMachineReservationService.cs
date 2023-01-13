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
        void ReserveMachineForUser(IEnumerable<PeriodMachineReservation> allMachineReservations, EmplooyeeRank rank,
            PeriodMachineReservation machineToReserve, Reservation reservation);

    }
}
