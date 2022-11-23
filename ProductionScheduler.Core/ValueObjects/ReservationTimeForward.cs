namespace ProductionScheduler.Core.ValueObjects;

public sealed record ReservationTimeForward
{
    private int _daysAhead = 7;
    public Date From { get; }
    public Date To { get; }

    public ReservationTimeForward(DateTimeOffset value)
    {
        // if hour is later than 13 from should be day + 1
        From = new Date(DateTime.UtcNow);   
        To = From.AddDays(_daysAhead);
        var from1 = From;
        var current = value;

        bool later = From < current.AddSeconds(11); // tutaj here #here

    }

    public override string ToString() => $"{From} -> {To}";
}