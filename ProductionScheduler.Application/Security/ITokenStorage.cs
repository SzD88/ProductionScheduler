using ProductionScheduler.Application.DTO;

namespace ProductionScheduler.Application.Security
{
    public interface ITokenStorage
    {
        void Set(JwtDto jwt);
        JwtDto Get();
    }
}
