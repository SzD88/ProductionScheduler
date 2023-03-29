using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ReserveMachineForService(  
        DateTime Date, 
        int Hour
        ) : ICommand; 
}
