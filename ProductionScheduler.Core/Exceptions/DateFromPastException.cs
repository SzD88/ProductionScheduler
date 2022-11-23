namespace ProductionScheduler.Core.Exceptions
{
    public sealed class DateFromPastException : CustomException
    {
        public DateFromPastException() : base("Date of reservation is from past")
        {
        }
    }
}