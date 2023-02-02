using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Application.Exceptions;

public class InvalidCredentialsException : CustomException
{
    public InvalidCredentialsException() : base("Invalid credentials.")
    {
    }
}