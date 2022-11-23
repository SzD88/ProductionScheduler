using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.ValueObjects;

public sealed record Date
{
    public DateTimeOffset Value { get; }

    public Date(DateTimeOffset value)  
    {
        if (value.Date < DateTime.UtcNow.Date)
        {
             throw new DateFromPastException();
        }
        
        Value = value; // date

    }

    public bool IsSunday() => Value.DayOfWeek == DayOfWeek.Sunday;
    public Date AddDays(int days) => new(Value.AddDays(days));

    public static implicit operator DateTimeOffset(Date date)
        => date.Value;

    public static implicit operator Date(DateTimeOffset value)
        => new(value);
    public static bool operator <(Date date1, Date date2)
        => date1.Value.Day < date2.Value.Day;

    public static bool operator >(Date date1, Date date2)
        => date1.Value.Day > date2.Value.Day;

    public static bool operator <=(Date date1, Date date2)
        => date1.Value.Day <= date2.Value.Day;

    public static bool operator >=(Date date1, Date date2)
        => date1.Value.Day >= date2.Value.Day;

    public static Date Now => new(DateTimeOffset.Now);

    public override string ToString() => Value.ToString("d"); 
}
