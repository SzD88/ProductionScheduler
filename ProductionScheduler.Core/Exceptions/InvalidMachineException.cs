namespace MachineReservations.Api.Exceptions;

public class InvalidMachineException : CustomException
{
    public InvalidMachineException() : base("Machine name is invalid.")
    {
    }
}