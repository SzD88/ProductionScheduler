using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
    internal class MSSqlUserRepository : IUserRepository
    { 
        private readonly DbSet<User> _users;

        public MSSqlUserRepository(ProductionSchedulerDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public Task<User> GetByIdAsync(UserId id)
            => _users.SingleOrDefaultAsync(x => x.Id == id);

        public Task<User> GetByEmailAsync(Email email)
            => _users.SingleOrDefaultAsync(x => x.Email == email);

        public Task<User> GetByUsernameAsync(UserName username)
            => _users.SingleOrDefaultAsync(x => x.UserName == username);

        public async Task AddAsync(User user)
            => await _users.AddAsync(user);
    } 
}

