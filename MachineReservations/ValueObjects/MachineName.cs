using MachineReservations.Api.Exceptions;

namespace MachineReservations.Core.ValueObjects;

public sealed record MachineName(string Value)
{
    public string Value { get; } = Value ?? throw new InvalidMachineException();

    public static implicit operator string(MachineName name)
        => name.Value;
    
    public static implicit operator MachineName(string value)
        => new(value);
}