using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;

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
            var machines = await _dbContext.Machines
                .Include(x => x.Reservations)
                .AsNoTracking()
                .ToListAsync();

            var cos = machines.Select(x => x.AsDto());
            return cos;
        }
    }
}
