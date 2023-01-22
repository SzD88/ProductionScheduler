using ProductionScheduler.Application.DTO;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.DAL.Handlers
{
    internal static class Extensions
    {
        public static MachineDto AsDto(this Machine entity)
        {
            var asDto = new MachineDto
            {
                Id = entity.Id.Value.ToString(),
                Name = entity.Name,
                From = entity.TimeForward.From.Value.DateTime,
                To = entity.TimeForward.To.Value.DateTime,
                Reservations = entity.Reservations.Select(x => new ReservationDto
                {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    EmployeeName = x is MachineReservation y ? y.EmployeeName : "service",  // #refactor = docelowo zmienia na service ale mialo
                    //tego nie byc , bez ttego walilo null reference 
                    Type = x is MachineReservation ? "machine" : "service", //#problem #here #refactor
                    Date = x.Date.Value.Date,
                    Hour = x.Hour.Value
                })
            };
            return asDto;
        }
    }
}

