using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Repositories
{
    public interface IPeriodMachineReservationRepository
    {
        Task<PeriodMachineReservation> GetAsync(MachineId id);
        Task<IEnumerable<PeriodMachineReservation>> GetAllAsync();
        public Task<IEnumerable<PeriodMachineReservation>> GetByPeriodAsync(ReservationTimeForward timeForward);
        //=> //throw new NotImplementedException(); // feature z c# 8 - pozwala tak zrobic zeby nie musiec zmieniac
        //zbyt wiele w przypadku wprowadzanej zmiany w trakcie, np teraz nie musze in memory modyfikowac tylko zmienie//
        //to co chce czyli sql service , #refactor
        Task CreateAsync(PeriodMachineReservation command);
        Task UpdateAsync(PeriodMachineReservation command);
        Task DeleteAsync(PeriodMachineReservation command);
    }
}
