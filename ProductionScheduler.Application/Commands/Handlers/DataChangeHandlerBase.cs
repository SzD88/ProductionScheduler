using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class DataChangeHandlerBase
    { 
        public async Task<Machine> GetMachineByReservationIdAsync(IMachinesRepository repository,  ReservationId reservationId)
        {
            var allMachines = await repository.GetAllAsync();

            return allMachines.SingleOrDefault(x => x.Reservations.Any
                 (r => r.Id == reservationId));
        }
    }
}
