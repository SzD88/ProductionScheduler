using MachineReservations.Api.Commands;
using MachineReservations.Api.DTO;

namespace MachineReservations.Api.Services
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
