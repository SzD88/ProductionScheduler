namespace ProductionScheduler.Core.Exceptions;

public sealed class EmptyEmployeeNameException : CustomException
{
    public EmptyEmployeeNameException() : base("Name was null during process")
    {
    }
}


