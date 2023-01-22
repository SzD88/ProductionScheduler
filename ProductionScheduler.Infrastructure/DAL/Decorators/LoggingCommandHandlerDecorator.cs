using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Infrastructure.DAL.Decorators
{
    internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        private readonly ICommandHandler<TCommand> _commandHandler;

        public async Task HandleAsync(TCommand command)
        {
            Console.WriteLine($"Processing a command {command.GetType().Name}");
            await _commandHandler.HandleAsync(command);
        }
    }
}
