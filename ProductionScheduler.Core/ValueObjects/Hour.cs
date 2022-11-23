using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.ValueObjects
{
    public record Hour
    {
        public short Value { get; }

        public Hour(short value)
        {
            Value = value;

            //if (value == null) 
            //{
            //    throw new EmptyHourException();
            //}
            if (Value is < 7 or > 14)
            {
                throw new InvalidHourException(value);
            }

        }

        public static implicit operator short(Hour hour)
        {
            return hour.Value;
        }

        public static implicit operator Hour(short hour)
        => new Hour(hour);
    }
}
