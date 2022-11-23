namespace ProductionScheduler.Core.ValueObjects;

public sealed record ReservationTimeForward
{
    private int _daysAhead = 7;
    public Date From { get; }
    public Date To { get; }

    public ReservationTimeForward(DateTimeOffset value)
    {
        //var pastDays = value.DayOfWeek is DayOfWeek.Sunday ? 7 : (int) value.DayOfWeek;
        //var remainingDays = 7 - pastDays;

        From = new Date(value);
        To = new Date(value.AddDays(_daysAhead));
        //version to end of next week after _daysAhead value
        // To = new Date(value.AddDays(remainingDays + _daysAhead));

    }

    public override string ToString() => $"{From} -> {To}";
}