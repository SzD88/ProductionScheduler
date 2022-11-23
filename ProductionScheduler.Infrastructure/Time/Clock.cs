namespace ProductionScheduler.Application.Services
{
    public class Clock : IClock
    {
        public DateTime Current() => DateTime.UtcNow;
        public Clock()
        {

        }
    }

}
