using ProductionScheduler.Application.Abstractions;


namespace ProductionScheduler.Application.Commands;

public record SignIn(string Email, string Password) : ICommand;
