using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public sealed class MachineReservation : Reservation
    {
        public  MachineReservation(ReservationId id, MachineId machineId, EmployeeName employeeName,
          Hour hour, Date date) : base (id, machineId, hour, date)
        {
            EmployeeName = employeeName;
        }

        public EmployeeName EmployeeName { get; private set; }

        public void ChangeEmployeeName(EmployeeName name)
        {
            EmployeeName = name;
        }

    }
   
}
