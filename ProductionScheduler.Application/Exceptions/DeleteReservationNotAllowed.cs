using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Application.Exceptions
{
    public class DeleteReservationNotAllowed : CustomException
    {
        public Guid Id { get; }
        public DeleteReservationNotAllowed(Guid id ) 
            : base($"Removal of reservation with id {id} is not allowed")
        {
            Id = id;
        } 
    }
}
