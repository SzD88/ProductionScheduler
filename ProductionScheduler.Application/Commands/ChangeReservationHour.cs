using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationHour(
        Guid ReservationId,
        Guid UserId,
        DateTime Date,
        int Hour
        ) : ChangeReservationDateAndTimeDto(Date, Hour), ICommand;

}
