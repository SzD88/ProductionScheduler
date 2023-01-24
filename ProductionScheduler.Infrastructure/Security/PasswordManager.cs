using Microsoft.AspNetCore.Identity;
using ProductionScheduler.Application.Security;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.Security
{
    internal sealed class PasswordManager : IPasswordManager
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordManager(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }
        public string Secure(string password)
        {
            return _passwordHasher.HashPassword(default, password);
        }

        public bool Validate(string password, string securedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(default, securedPassword, password)
                   is PasswordVerificationResult.Success;
        }
    }
} 
