using ProductionScheduler.Core.Exceptions;
using System.Runtime.Serialization;

namespace ProductionScheduler.Core.ValueObjects;

public sealed record EmployeeName(string value)
{
    private bool isNUll = IsInUs(value); //#refactor zrozum i usun lub cos 
     
    public string Value { get; } = value ?? throw new InvalidEmployeeNameException();
//     public string Val2ue { get; } = null ?? throw new EmptyEmployeeNameException();


    public static implicit operator string(EmployeeName name)
        => name.Value ;

    public static implicit operator EmployeeName(string value)
        => new(value);

    private static bool IsInUs(string testValue)
    { 
        if (testValue == null)
        {
            throw new EmptyEmployeeNameException();
        }
        return false;
    }
}



