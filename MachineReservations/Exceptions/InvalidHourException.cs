namespace MachineReservations.Api.Exceptions
{
    public sealed class InvalidHourException : CustomException
    {
        public string Hour { get;   }
        public InvalidHourException(string hour)
            : base($"Hour: {hour} is invalid")
        {
            Hour = hour;
        }
    }

}

