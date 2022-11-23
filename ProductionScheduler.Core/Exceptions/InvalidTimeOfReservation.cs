namespace ProductionScheduler.Core.Exceptions
{
    public sealed class InvalidTimeOfReservation : CustomException
    {
        public InvalidTimeOfReservation() : base("Time of reservation is invalid")
        {
        }
    }

}