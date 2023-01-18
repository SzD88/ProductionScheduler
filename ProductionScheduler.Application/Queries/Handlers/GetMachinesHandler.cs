using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Queries.Handlers
{
    public sealed class GetMachinesHandler
         : IQueryHandler<GetMachines, IEnumerable<MachineDto>>
    {
        public Task<IEnumerable<MachineDto>> HandleAsync(GetMachines query)
        {
            throw new NotImplementedException();
        }
    }
}
