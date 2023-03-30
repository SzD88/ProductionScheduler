using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Security
{
    public interface IAuthenticator
    {
        JwtDto CreateToken(Guid userId, string role);
    }
}
