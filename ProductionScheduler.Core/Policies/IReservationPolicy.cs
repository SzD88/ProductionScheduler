using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Core.Policies
{
    public interface IReservationPolicy
    {
        bool CanBeApplied(EmplooyeeRank rank);
        bool CanReserve(IEnumerable<PeriodMachineReservation> periodMachineReservations,
             EmployeeName name);
    }
}
