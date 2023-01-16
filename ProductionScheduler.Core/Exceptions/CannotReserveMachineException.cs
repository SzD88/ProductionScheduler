using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Exceptions;

public sealed class CannotReserveMachineException : CustomException
{
    public MachineId Id { get; }
    public CannotReserveMachineException(MachineId id) 
        : base($"Cannot reserve machine with id: {id} .")
    {
        Id = id;
    }
}