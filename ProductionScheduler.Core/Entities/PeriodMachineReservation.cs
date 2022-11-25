using ProductionScheduler.Core.Exceptions;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public class PeriodMachineReservation
    {
        private readonly HashSet<Reservation> _reservations = new HashSet<Reservation>();
        public MachineId Id { get; }
        public ReservationTimeForward TimeForward { get; }
        public MachineName Name { get; }
        public IEnumerable<Reservation> Reservations => _reservations;

        public PeriodMachineReservation(MachineId id, ReservationTimeForward week, MachineName name)
        {
            Id = id;
            TimeForward = week;
            Name = name;
        }
        private PeriodMachineReservation()
        {

        }

        public void AddReservation(Reservation reservation, Date now)
        {
            var date = reservation.Date;
            var from = TimeForward.From.Value.Date;
            var to = TimeForward.To.Value.Date;
            //#tutaj 
            //week zadeklarowano na poczaatku servisu
            // czyli week masz od 14-20
            var one = date < from;
            var two = date > to;
            var three = date < now;
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
            var reservationDateAlredyExists = Reservations.Any(
                x => x.Date == reservation.Date);

            if (reservationDateAlredyExists)
            {
                var reservationHourAlredyExists = Reservations.Any(
                 x => x.Hour == reservation.Hour);
                if (reservationHourAlredyExists)
                {
                    throw new MachineAlredyReservedException(Name, reservation.Date.Value.Date);
                }
            }
            _reservations.Add(reservation);
        }

        public void RemoveReservation(ReservationId id)
       => _reservations.RemoveWhere(x => x.Id == id);
    }
}
