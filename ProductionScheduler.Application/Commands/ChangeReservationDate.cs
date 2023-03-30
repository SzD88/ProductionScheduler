using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationDate( 
        Guid ReservationId, 
        DateTime Date 
        ) : ICommand; 
}
