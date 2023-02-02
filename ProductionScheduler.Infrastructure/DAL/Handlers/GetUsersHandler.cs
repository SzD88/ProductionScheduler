using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;

namespace ProductionScheduler.Infrastructure.DAL.Handlers
{

    internal sealed class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
    {
        private readonly ProductionSchedulerDbContext _dbContext;

        public GetUsersHandler(ProductionSchedulerDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
            => await _dbContext.Users
                .AsNoTracking()
                .Select(x => x.AsDto())
                .ToListAsync();
    }
}