using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Services
{
    public interface IReservationService
    { 
        Task<ReservationDto> GetAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<Guid?> CreateAsync(CreateReservation command);
        Task<bool> UpdateAsync(ChangeReservationHour command);
        Task<bool> DeleteAsync(DeleteReservation command);
    }
}
