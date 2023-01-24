using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(UserId id);
        Task<User> GetByEmailAsync(Email email);
        Task<User> GetByUsernameAsync(UserName username);
        Task AddAsync(User user);

    }
}
