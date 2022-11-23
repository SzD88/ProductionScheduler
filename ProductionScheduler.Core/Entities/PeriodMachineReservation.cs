using ProductionScheduler.Core.Exceptions;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public class PeriodMachineReservation
    {
        private readonly HashSet<Reservation> _reservations = new HashSet<Reservation>();
        public MachineId Id { get; }
        public ReservationTimeForward Week { get; }
        public MachineName Name { get; }
        public IEnumerable<Reservation> Reservations => _reservations;

        public PeriodMachineReservation(MachineId id, ReservationTimeForward week, MachineName name)
        {
            Id = id;
            Week = week;
            Name = name;
        }

        public void AddReservation(Reservation reservation, Date now)
        {
            var date = reservation.Date;
            var from = Week.From;
            var to = Week.To;
            //#tutaj 
            //week zadeklarowano na poczaatku servisu
            // czyli week masz od 14-20
            var isInvalidDate = date < from  //14
                || date > to //20
                || date < now; // .Date? // sprawdza dzien 

            if (isInvalidDate)
            {
                throw new InvalidReservationDateException(reservation.Date.Value.Date);
            }
            if (date.IsSunday())
            {
                throw new ReservationDayIsSundayException();
            }
            var reservationAlredyExists = Reservations.Any(
                x => x.Date == reservation.Date);
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
