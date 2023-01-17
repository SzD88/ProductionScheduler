using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Services
{
    public interface IReservationService
    { 
        Task<ReservationDto> GetAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<Guid?> ReserveForEmployeeAsync(ReserveMachineForEmployee command);
        Task ReserveAllMachinesForServiceAsync(ReserveMachineForService command);
        Task<bool> ChangeReservationHourAsync(ChangeReservationHour command);
        Task<bool> ChangeReservationDateAsync(ChangeReservationDate command);
        Task<bool> DeleteAsync(DeleteReservation command);
    }
}
