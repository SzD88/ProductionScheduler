﻿using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Application.Commands
{
    public record SignUp(
        Guid UserId, 
        string Email, 
        string UserName, 
        string Password, 
        string FullName, 
        string Role) : ICommand; 
}
