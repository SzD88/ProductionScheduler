using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Handlers

{
    internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly ProductionSchedulerDbContext _dbContext;

        public GetUserHandler(ProductionSchedulerDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var userId = new UserId(query.UserId);
            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);

            return user?.AsDto();
        }
    }
}