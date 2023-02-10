using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForEmployee( // names should be behavior centric #refactor 

        Guid MachineId,
        Guid ReservationId,
        Guid UserId, 
        DateTime Date,
        int Hour
        ) 
        : ICommand; // implement interface

}
