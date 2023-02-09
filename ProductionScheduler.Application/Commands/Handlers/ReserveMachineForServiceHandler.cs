using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.DomainServices;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class ReserveMachineForServiceHandler : ICommandHandler<ReserveMachineForService>
    { 
        private readonly IMachinesRepository _repository;
        private readonly IMachineReservationService _reservationService;

        public ReserveMachineForServiceHandler(IMachinesRepository repository,
            IMachineReservationService machineReservationService)
        {
            _repository = repository;
            _reservationService = machineReservationService;
        }
        public async Task HandleAsync(ReserveMachineForService command)
        { 
            var machines = (await _repository.GetAllAsync()).ToList();

            foreach (var item in machines)
            { 
                if (item is null)
                {
                    throw new MachineNotFoundException(item.Id);
                }
            } 

            var date = new Date(command.Date);
            var hour = new Hour(command.Hour);
             
            _reservationService.ReserveMachineForService(machines,
                date, hour);
              
            var tasks = machines.Select(x => _repository.UpdateAsync(x));
            await Task.WhenAll(tasks);  
        }
    }
}
