using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Application.Services;

namespace MachineReservations.Tests.Unit.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current()
        {
            return new DateTime(2022, 11, 22);
        }
    }
}
