using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.ValueObjects
{
    public record Hour
    {
        public int Value { get; }

        public Hour(int value)
        {
            Value = value;
             
            if (Value is < 7 or > 14)
            {
                throw new InvalidHourException(value);
            } 
        }

        public static implicit operator int(Hour hour)
        {
            return hour.Value;
        }

        public static implicit operator Hour(int hour)
        => new Hour(hour);
    }
}
