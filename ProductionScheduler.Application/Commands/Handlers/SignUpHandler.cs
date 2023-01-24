using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Security;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;

namespace ProductionScheduler.Application.Commands.Handlers
{
    internal sealed class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;

        public SignUpHandler(IUserRepository userRepository, IPasswordManager passwordManager ,IClock clock )
        {
            _clock = clock;
            this._userRepository = userRepository;
            _passwordManager = passwordManager;
        }
        public async Task HandleAsync(SignUp command)
        {
            //validate input

            //validate if user alredy exists (email or username )

            //create user
            var securedPassword = _passwordManager.Secure(command.Password );
            var user = new User(command.UserId, command.Email, command.UserName, securedPassword, command.FullName, command.Role, _clock.Current()) ;
            //Save to db


        }
    }
}
