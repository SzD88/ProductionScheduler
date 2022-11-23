namespace ProductionScheduler.Core.ValueObjects;

public sealed record MachineId
{
    public Guid Value { get; }

    public MachineId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new Exception(); // value // #refactor 
        }

        Value = value;
    }

    public static MachineId Create() => new(Guid.NewGuid());

    public static implicit operator Guid(MachineId date)
        => date.Value;

    public static implicit operator MachineId(Guid value)
        => new(value);

    public override string ToString() => Value.ToString("N");
}