namespace ProductionScheduler.Application.Services
{
    public interface IClock
    {
        DateTimeOffset Current();
    }
}
