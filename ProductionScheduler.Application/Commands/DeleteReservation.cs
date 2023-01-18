using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record DeleteReservation(

        Guid ReservationId

          ) : ICommand;

}
