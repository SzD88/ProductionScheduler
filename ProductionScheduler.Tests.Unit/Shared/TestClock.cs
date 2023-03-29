using ProductionScheduler.Core.Abstractions;
using System;

namespace MachineReservations.Tests.Unit.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current()
        {
          // return new DateTime(2023, 01, 17, 12, 0, 0);
            return DateTime.UtcNow.AddDays(0);
        }
    }
}
