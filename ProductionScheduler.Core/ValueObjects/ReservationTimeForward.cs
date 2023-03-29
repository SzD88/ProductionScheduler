namespace ProductionScheduler.Core.ValueObjects;
public sealed record ReservationTimeForward
{
    private int _daysAhead = 7; // #refactor 
    public Date From { get; }
    public Date To { get; } 
    public ReservationTimeForward(DateTimeOffset value)
    { 
        From = new Date(DateTime.UtcNow);

        if (value.Hour > 13)
        { 
            From.AddDays(1);
            From = From.IsSunday() ? From.AddDays(1) : From.AddDays(0);
        }

        To = From.AddDays(_daysAhead);

        To = To.SetHour(value.Hour);
    } 
    public override string ToString() => $"{From} -> {To}";
}