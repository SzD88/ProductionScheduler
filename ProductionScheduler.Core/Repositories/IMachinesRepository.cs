using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Repositories
{
    public interface IMachinesRepository
    {
        Task<Machine> GetAsync(MachineId id);
        Task<IEnumerable<Machine>> GetAllAsync();
        public Task<IEnumerable<Machine>> GetByPeriodAsync(ReservationTimeForward timeForward);
        Task CreateAsync(Machine command);
        Task UpdateAsync(Machine command);
        Task DeleteAsync(Machine command);
    }
}
