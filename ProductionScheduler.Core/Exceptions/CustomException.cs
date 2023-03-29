using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Core.Exceptions;

public abstract class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {

    }
}
