using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForService( // names should be behavior centric #refactor 

       //  Guid MachineId,
        
        DateTime Date,
        
        short Hour
        ) : ICommand;

}
