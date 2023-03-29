using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Application.Exceptions
{
    public class MachineNotFoundException : CustomException
    {
        public MachineNotFoundException(Guid id  ) 
            : base($"Machine with id : {id} was not found ")
        {
            Id = id;
        } 
        public Guid Id { get; }
    }
}
