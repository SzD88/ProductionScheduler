namespace MachineReservations.Api.Exceptions
{
    public sealed class InvalidHourException : CustomException
    {
        public short Hour { get; }
        public InvalidHourException(short hour)
            : base($"Hour: {hour} is invalid")
        {
            Hour = hour;
        }
    }

}

