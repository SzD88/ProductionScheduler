using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Application.Exceptions
{
    public class ReservationNotFoundException : CustomException
    {
        public Guid Id { get; }

        public ReservationNotFoundException(Guid id)
            : base($"Reservation with id : {id} was not found ")
        {
            Id = id;
        } 
    }
}
