namespace ProductionScheduler.Application.Commands
{
    public record CreateReservation(

        Guid MachineId,
        Guid ReservationId,
        DateTime Date,
        string EmployeeName,
        short Hour
        );

}
