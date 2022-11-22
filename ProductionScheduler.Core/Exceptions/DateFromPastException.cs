using MachineReservations.Api.Exceptions;

namespace MachineReservations.Api.ValueObjects
{
    public sealed class DateFromPastException : CustomException
    {
        public DateFromPastException() : base("Date of reservation is from past")
        {
        }
    }
}