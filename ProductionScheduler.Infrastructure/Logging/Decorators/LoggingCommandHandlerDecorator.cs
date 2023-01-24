using System.Diagnostics;
using Humanizer;
using Microsoft.Extensions.Logging;
using ProductionScheduler.Application.Abstractions;

namespace ProductionScheduler.Infrastructure.Logging.Decorators
{
    internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        private readonly ICommandHandler<TCommand> _commandHandler;
        private readonly ILogger<ICommandHandler<TCommand>> _logger;
        public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, ILogger<ICommandHandler<TCommand>> logger)
        {
            _commandHandler = commandHandler;
            _logger = logger;
        } 

        public async Task HandleAsync(TCommand command)
        {
            var commandName = typeof(TCommand).Name.Underscore();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogInformation("Started handling a command{commandName} ...", commandName); // serilog #32/20 - usunieto "$"
            //    Console.WriteLine($"Processing a command {command.GetType().Name}");
            await _commandHandler.HandleAsync(command);
            stopWatch.Stop();
            _logger.LogInformation("Completed handling a command {commandName} ... time of process : {Elapsed}", commandName, stopWatch.Elapsed);

        }
    }
}
