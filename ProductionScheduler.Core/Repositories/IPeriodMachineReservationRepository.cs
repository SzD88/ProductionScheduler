using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Repositories
{
    public interface IPeriodMachineReservationRepository
    {
        Task<PeriodMachineReservation> GetAsync(MachineId id);
        Task<IEnumerable<PeriodMachineReservation>> GetAllAsync();
        Task CreateAsync(PeriodMachineReservation command);
        Task UpdateAsync(PeriodMachineReservation command);
        Task DeleteAsync(PeriodMachineReservation command);
    }
}
