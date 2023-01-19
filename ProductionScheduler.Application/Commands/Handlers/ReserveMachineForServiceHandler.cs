using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.DomainServices;
using ProductionScheduler.Core.Entities;
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
            // version with getbyperiod 
            //var timeforward = new ReservationTimeForward(command.Date);
            //var machines = (await _allMachines.GetByPeriodAsync(timeforward)).ToList();

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

            foreach (var item in machines)
            {
                await _repository.UpdateAsync(item);
            } 
        }
    }
}
