using ProductionScheduler.Core.Abstractions;

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
