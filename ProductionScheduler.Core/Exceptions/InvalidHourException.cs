namespace ProductionScheduler.Core.Exceptions;

public sealed class InvalidHourException : CustomException
{
    public int Hour { get; }
    public InvalidHourException(int hour)
        : base($"Hour: {hour} is invalid")
    {
        Hour = hour;
    }
}


