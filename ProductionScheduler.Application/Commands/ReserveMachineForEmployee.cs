using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForEmployee(  

        Guid MachineId,
        Guid ReservationId,
        Guid UserId, 
        DateTime Date,
        int Hour,
        string EmployeeName

        )
        : ICommand;  

}
