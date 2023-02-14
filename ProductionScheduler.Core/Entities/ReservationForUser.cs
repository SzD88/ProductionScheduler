using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public sealed class ReservationForUser : Reservation 
    {
        public UserId UserId { get; private set; } 
        public EmployeeName EmployeeName { get; private set; }

        public ReservationForUser()
        { 
        }
        public  ReservationForUser(ReservationId id, MachineId machineId, UserId userId, EmployeeName employeeName,
          Hour hour, Date date) : base (id, machineId, hour, date)
        {
            UserId = userId;
            EmployeeName = employeeName;
        } 

        public void ChangeEmployeeName(EmployeeName name)
        {
            EmployeeName = name;
        } 
    } 
}
