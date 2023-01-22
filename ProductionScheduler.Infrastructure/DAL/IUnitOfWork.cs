using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal interface IUnitOfWork
    {
        // dowolna operacja synchroniczna, dowolny callback
        Task ExecuteAsync(Func<Task> action);
    }
}
