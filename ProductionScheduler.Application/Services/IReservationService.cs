using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Services
{
    public interface IReservationService
    {

        ReservationDto Get(Guid id);
        IEnumerable<ReservationDto> GetAllWeekly();
        Guid? Create(CreateReservation command);
        bool Update(ChangeReservationHour command);
        bool Delete(DeleteReservation command);
    }
}
