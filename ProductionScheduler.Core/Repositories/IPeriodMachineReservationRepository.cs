using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Repositories
{
    public interface IPeriodMachineReservationRepository
    {
        Task<MachineToReserve> GetAsync(MachineId id);
        Task<IEnumerable<MachineToReserve>> GetAllAsync();
        public Task<IEnumerable<MachineToReserve>> GetByPeriodAsync(ReservationTimeForward timeForward);
        //=> //throw new NotImplementedException(); // feature z c# 8 - pozwala tak zrobic zeby nie musiec zmieniac
        //zbyt wiele w przypadku wprowadzanej zmiany w trakcie, np teraz nie musze in memory modyfikowac tylko zmienie//
        //to co chce czyli sql service , #refactor
        Task CreateAsync(MachineToReserve command);
        Task UpdateAsync(MachineToReserve command);
        Task DeleteAsync(MachineToReserve command);
    }
}
