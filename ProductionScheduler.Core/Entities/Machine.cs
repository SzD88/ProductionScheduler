using ProductionScheduler.Core.Exceptions;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{ 
    public class Machine
    {
        private readonly HashSet<Reservation> _reservations = new HashSet<Reservation>();
        public MachineId Id { get; }
        public ReservationTimeForward TimeForward { get; }
        public MachineName Name { get; }
        public IEnumerable<Reservation> Reservations => _reservations;

        public Machine(MachineId id, ReservationTimeForward timeForward, MachineName name)
        {
            Id = id;
            TimeForward = timeForward;
            Name = name;
        }
        private Machine()
        {

        } 
        internal void AddReservation(Reservation reservation, Date now)
        {
            var date = reservation.Date;
            var from = TimeForward.From.Value.Date;
            var to = TimeForward.To.Value.Date; 
            var one = date < from;
            var two = date > to;
            var three = date < now;
            var isInvalidDate = date < from  
                || date > to  
                || date < now;  

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
                    throw new MachineAlredyReservedException(Name, reservation.Date.Value.Date.Date, reservation.Hour.Value);
                }
            }
            _reservations.Add(reservation);
        }

        public void RemoveReservation(ReservationId id)
       => _reservations.RemoveWhere(x => x.Id == id);

        public void RemoveReservations(IEnumerable<Reservation> reservations)
       => _reservations.RemoveWhere(x => reservations.Any(r => r.Id == x.Id));
    }
}
