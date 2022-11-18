using MachineReservations.Api.Exceptions;

namespace MachineReservations.Api.ValueObjects
{
    public record Hour
    {
        public string Value { get; }   

        public Hour(string value)
        {
            Value = value;

            if (string.IsNullOrWhiteSpace(value)) 
            {
                throw new EmptyHourException();
            }
            if (Value.Length is  > 2)
            {
                throw new InvalidHourException(value);
            }

        }

        public static implicit operator string(Hour hour)
        {
        return hour?.Value;
        }

        public static implicit operator Hour(string hour)
        => new Hour(hour);
    }
}
