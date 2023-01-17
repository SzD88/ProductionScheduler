using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Abstractions;

namespace MachineReservations.Tests.Unit.Shared
{
    public class TestClock : IClock
    {
        public DateTimeOffset Current()
        {
          // return new DateTime(2023, 01, 17, 12, 0, 0);
            return DateTime.UtcNow.AddDays(0);
        }
    }
}
