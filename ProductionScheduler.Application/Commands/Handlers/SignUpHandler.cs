using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Application.Security;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    internal sealed class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;

        public SignUpHandler(IUserRepository userRepository, IPasswordManager passwordManager, IClock clock)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _clock = clock;

        }
        public async Task HandleAsync(SignUp command)
        { 
            var userId = new UserId(command.UserId);
            var userId2 = new UserId(Guid.NewGuid());
            var email = new Email(command.Email);
            var userName = new UserName(command.UserName);
            var password = new Password(command.Password);
            var fullName = new FullName(command.FullName);
            var role = string.IsNullOrWhiteSpace(command.Role)
                ? Role.User() : new Role(command.Role);
            //validate if user alredy exists (email or username)
            if (await _userRepository.GetByEmailAsync(email) is not null)
            {
                throw new EmailAlreadyInUseException(email);
            } 
            if (await _userRepository.GetByUsernameAsync(userName) is not null)
            {
                throw new UsernameAlreadyInUseException(userName);
            }
            //create user
            var securedPassword = _passwordManager.Secure(command.Password);
            var user = new User(userId, email, userName, securedPassword, fullName, role, _clock.Current());
            //Save to db
            await _userRepository.AddAsync(user);


        }
    }
}
