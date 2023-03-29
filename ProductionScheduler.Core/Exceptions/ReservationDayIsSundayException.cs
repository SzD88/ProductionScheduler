namespace ProductionScheduler.Core.Exceptions;

public sealed class ReservationDayIsSundayException : CustomException
{
    public ReservationDayIsSundayException() : base("You cannot book this machine for sunday")
    {
    }
}
