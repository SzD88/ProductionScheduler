using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationHour(
        Guid ReservationId,
        int Hour
        ) : ICommand;

}
