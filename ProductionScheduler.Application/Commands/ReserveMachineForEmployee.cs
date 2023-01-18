using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForEmployee( // names should be behavior centric #refactor 

        Guid MachineId,
        Guid ReservationId,
        DateTime Date,
        string EmployeeName,
        short Hour
        ) 
        : ICommand; // implement interface

}
