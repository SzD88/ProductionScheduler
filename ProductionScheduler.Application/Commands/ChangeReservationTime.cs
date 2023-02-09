using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationTime(

        Guid ReservationId,
        Guid UserId,
        DateTime Date, 
        int Hour
        ) : ICommand;

}
