namespace ProductionScheduler.Application.Security
{
    public interface IPasswordManager
    {
        string Secure(string password);
        bool Validate(string password, string SecuredPassword);
    }
}
