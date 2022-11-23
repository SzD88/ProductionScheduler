using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Core.Exceptions
{

    public sealed class EmptyHourException : CustomException
    {
        public EmptyHourException() : base("Hour of reservation is empty")
        {
        }
    }

}

