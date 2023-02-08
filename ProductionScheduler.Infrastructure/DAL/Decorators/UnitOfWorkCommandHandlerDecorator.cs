using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;

namespace ProductionScheduler.Infrastructure.DAL.Decorators
{
    // ale my chcemy zeby globalnie byl zaaplikowany ten dekorator, dla kazdej jednej komendy ten dekorator byl wykonywany 
    // zeby to zrobic z ICommandHandler usuwamy silnie typowany generator i zamienamy na open Generic T
    // internal sealed class UnitOfWorkCommandHandlerDecorator : ICommandHandler<ReserveMachineForService>
    // 
    // T musi byc typem referencyjnym i musi byc komendą , taka sama definicja jak command handler 
    internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _commandHandler;
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IUnitOfWork unitOfWork)
        {
            _commandHandler = commandHandler;
            _unitOfWork = unitOfWork;
        }
         
        public async Task HandleAsync(TCommand command)
        {
            await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command));
        }

       
    }
}
