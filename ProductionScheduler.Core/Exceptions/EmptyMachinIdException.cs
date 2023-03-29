namespace ProductionScheduler.Core.Exceptions;

public sealed class EmptyMachinIdException : CustomException
{
    public EmptyMachinIdException() : base("Machine ID is empty")
    {
    }
} 

