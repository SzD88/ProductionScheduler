namespace ProductionScheduler.Core.ValueObjects;

public sealed record ReservationTimeForward
{
    private int _daysAhead = 7;
    public Date From { get; }
    public Date To { get; }

    public ReservationTimeForward(DateTimeOffset value)
    {
        // dostajesz clock.current - czyli obecny czas, w celu sprawdzenia
        // if hour is later than 13 from should be day + 1
        From = new Date(DateTime.UtcNow);

        if (value.Hour > 13)
        {
            //if reservation happens after 13
         From.AddDays(1);
         From = From.IsSunday() ? From.AddDays(1) : From.AddDays(0);
        }

        To = From.AddDays(_daysAhead);
         
        // value powstaje wczesniej a tu jest przekazaane ! 
        
    }

    public override string ToString() => $"{From} -> {To}";
}