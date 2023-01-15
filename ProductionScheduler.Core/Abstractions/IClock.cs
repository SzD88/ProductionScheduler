namespace ProductionScheduler.Core.Abstractions
{
    public interface IClock
    {
        DateTimeOffset Current();
    }
}
