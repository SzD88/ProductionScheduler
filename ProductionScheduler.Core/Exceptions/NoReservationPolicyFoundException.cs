using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Exceptions;

public sealed class NoReservationPolicyFoundException : CustomException
{
    public EmplooyeeRank Rank { get;   }
    public NoReservationPolicyFoundException(EmplooyeeRank rank) 
        : base($"No reservation policy for {rank} has been found.")
    {
        Rank = rank;
    }
}