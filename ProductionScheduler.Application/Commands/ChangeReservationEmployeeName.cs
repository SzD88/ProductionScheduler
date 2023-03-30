using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationEmployeeName( 
        Guid ReservationId, 
        string EmployeeName 
        ) : ICommand; 
}
