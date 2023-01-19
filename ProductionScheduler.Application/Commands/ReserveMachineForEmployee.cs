using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForEmployee( // names should be behavior centric #refactor 

        Guid MachineId,
        Guid ReservationId,
        DateTime Date,
        string EmployeeName,
        int Hour
        ) 
        : ICommand; // implement interface

}
