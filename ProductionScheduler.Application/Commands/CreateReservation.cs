namespace MachineReservations.Api.Commands
{
    public record CreateReservation(

        Guid MachineId, 
        Guid ReservationId,
        DateTime Date, 
        string EmployeeName, 
        short Hour    
        );
     
}
