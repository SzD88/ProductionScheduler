namespace ProductionScheduler.Core.ValueObjects;

//This class is defining how long from now  machine can be reserved MAX ,
//// I want to try export this setting somehow

public sealed record ReservationTimeForward
{
    private int _daysAhead = 7; // #refactor 
    public Date From { get; }
    public Date To { get; }

    // chcesz jeszcze zaokr�gla� do ca�ego dnia, �eby nie tworzy�o nowej instancji maszyny z jej rezerwacjami caly czas np w DatabaseInitializer.cs
    // chcesz to jakos sprawdza� , bo moze obecnie byc sytuacja ze To, jest ustawione na x dni ale do godziy ustawienia np 7 rano 
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

        To = To.SetHour(value.Hour);
    }

    public override string ToString() => $"{From} -> {To}";
}