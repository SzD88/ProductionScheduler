using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}
