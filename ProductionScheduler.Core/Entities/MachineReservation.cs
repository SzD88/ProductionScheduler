using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public sealed class MachineReservation : Reservation
    {
        public EmployeeName EmployeeName { get; private set; }

        public MachineReservation()
        { 
        }
        public  MachineReservation(ReservationId id, MachineId machineId, EmployeeName employeeName,
          Hour hour, Date date) : base (id, machineId, hour, date)
        {
            EmployeeName = employeeName;
        } 

        public void ChangeEmployeeName(EmployeeName name)
        {
            EmployeeName = name;
        }
     

    }
   
}
