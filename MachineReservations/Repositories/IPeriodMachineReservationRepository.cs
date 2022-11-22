using MachineReservations.Api.Entities;
using MachineReservations.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineReservations.Repositories
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
