using MachineReservations.Api.Controllers.Models;
using MachineReservations.Api.Exceptions;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Core.ValueObjects;

namespace MachineReservations.Api.Entities
{
    public class WeeklyMachineReservation
    {
        private readonly HashSet<Reservation> _reservations = new HashSet<Reservation>();
        public MachineId Id { get; } 
        public Week Week { get;   }
        public MachineName Name { get;  }
        public IEnumerable<Reservation> Reservations => _reservations;

        public WeeklyMachineReservation(MachineId id, Week week, MachineName name)
        {
            Id = id;
            Week = week;
            Name = name;
        }

        public void AddReservation(Reservation reservation, Date now)
        {
            var isInvalidDate = reservation.Date < Week.From
                || reservation.Date > Week.To
                || reservation.Date < now ; // .Date?
             
            if (isInvalidDate)
            {
                throw new InvalidReservationDateException(reservation.Date.Value.Date);
            } 
            var reservationAlredyExists = Reservations.Any (
                x => x.Date  == reservation.Date);
            if (reservationAlredyExists)
            {
                throw new MachineAlredyReservedException(Name, reservation.Date.Value.Date);
            }
            _reservations.Add(reservation);
        }
        
        public void RemoveReservation(ReservationId id)
       => _reservations.RemoveWhere(x => x.Id == id);
    }
}
