using ProductionScheduler.Core.Abstractions;
using System;

namespace MachineReservations.Tests.Unit.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current()
        { 
            return DateTime.UtcNow.AddDays(0);
        }
    }
}
