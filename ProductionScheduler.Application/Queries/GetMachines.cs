using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Queries
{
    public class GetMachines : IQuery<IEnumerable<MachineDto>>
    {
        public DateTime? Date { get; set; }
    }
}
