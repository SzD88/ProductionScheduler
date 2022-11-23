using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Core.Repositories
{
    public interface IPeriodMachineReservationRepository
    {
        PeriodMachineReservation Get(MachineId id);
        IEnumerable<PeriodMachineReservation> GetAll();
        void Create(PeriodMachineReservation command);
        void Update(PeriodMachineReservation command);
        void Delete(PeriodMachineReservation command);
    }
}
