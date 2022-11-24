namespace ProductionScheduler.Application.Services
{
    public class Clock : IClock
    {
        public DateTimeOffset Current() => DateTime.UtcNow;
        public Clock()
        {

        }
    }

}
