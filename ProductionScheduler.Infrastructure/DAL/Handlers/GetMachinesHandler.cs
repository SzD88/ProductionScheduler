using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Handlers
{
    internal sealed class GetMachinesHandler
         : IQueryHandler<GetMachines, IEnumerable<MachineDto>>
    {
        private readonly ProductionSchedulerDbContext _dbContext;

        public GetMachinesHandler(ProductionSchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MachineDto>> HandleAsync(GetMachines query)
        {
          //  var timeForward = query.Date.HasValue ? // # to nie ma prawa dzialac #problem #here
          ////        new ReservationTimeForward(query.Date.Value) : null;
            var machines = await _dbContext.Machines
                //#refactor - conditional predicate - to taki sneaki trick - jezeli powyzsze timeForward jest null,
                //wtedy ponizsze where timeForward == null bo null == nul wiec -> true wiec wykona ten where
                //albo po drugiej wartosci xtimeforward =timeforward - wtedy tez leci where
                // co to znaczy nie wiem co mam powiedziec nie wiem nie moge jesc nie moge spac
          //      .Where(x => timeForward == null || x.TimeForward == timeForward) //#30 1h01min
                .Include(x => x.Reservations)
                .AsNoTracking()
                .ToListAsync();

            var cos = machines.Select(x => x.AsDto());
            return cos;
        }
    }
}
