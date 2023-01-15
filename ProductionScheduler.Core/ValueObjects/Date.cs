using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.ValueObjects;

public sealed record Date
{
    public DateTimeOffset Value { get; }

    public Date(DateTimeOffset value)
    {
        if (value.Date < DateTime.UtcNow.Date)
        {
          //  throw new DateFromPastException();
        }

        Value = value; // date

    }

    public bool IsSunday() => Value.DayOfWeek == DayOfWeek.Sunday;
    public Date AddDays(int days) => new(Value.AddDays(days));
    public Date SetHour(int hour)
    {
        int toAdd = 0;
        // get hour
        if (Value.Hour < 14 || Value.Hour > 0)
        {
            var currHour = Value.Hour;
            // 14-hour
            toAdd = 14 - currHour;//#refactor 
        }
        return new Date(Value.AddHours(toAdd));

    }
    public static implicit operator DateTimeOffset(Date date)
        => date.Value;

    public static implicit operator Date(DateTimeOffset value)
        => new(value);
    public static bool operator <(Date date1, Date date2)
        => date1.Value.Day < date2.Value.Day;
    public static bool operator <(DateTimeOffset date1, Date date2)
       => date1 < date2.Value;
    public static bool operator >(DateTimeOffset date1, Date date2)
      => date1 > date2.Value;
    public static bool operator <(Date date1, DateTimeOffset date2)
       => date1.Value < date2;
    public static bool operator >(Date date1, DateTimeOffset date2)
      => date1.Value > date2;
    public static bool operator >(Date date1, Date date2)
        => date1.Value.Day > date2.Value.Day;

    public static bool operator <=(Date date1, Date date2)
        => date1.Value.Day <= date2.Value.Day;

    public static bool operator >=(Date date1, Date date2)
        => date1.Value.Day >= date2.Value.Day;

    public static Date Now => new(DateTimeOffset.Now);

    public override string ToString() => Value.ToString("d");
}
