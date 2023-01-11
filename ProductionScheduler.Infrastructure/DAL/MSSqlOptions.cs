using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Infrastructure.DAL
{
    public class MSSqlOptions
    {
        public string ConnectionString { get; set; }
        public bool   ApplyMigration { get; set; }
    }
}
