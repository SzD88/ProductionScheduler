using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.DomainServices;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class ReserveMachineForEmployeeHandler : ICommandHandler<ReserveMachineForEmployee>
    {

        private readonly IClock _clock;
        private readonly IMachinesRepository _allMachines;
        private readonly IMachineReservationService _machineReservationService;


        public ReserveMachineForEmployeeHandler(IClock clock, IMachinesRepository repository,
            IMachineReservationService machineReservationService)
        {
            _clock = clock;
            _allMachines = repository;
            _machineReservationService = machineReservationService;
        }
        public async Task HandleAsync(ReserveMachineForEmployee command)
        {
            var machineId = new MachineId(command.MachineId);
            var allReservations = await _allMachines.GetAllAsync();
 
            var machines = (await _allMachines.GetAllAsync()).ToList();

            var machineToReserve = machines.SingleOrDefault(x => x.Id == machineId);
            if (machineToReserve is null)
            {
                throw new MachineNotFoundException(machineId);
            }

            var reservation = new ReservationForUser(command.ReservationId, command.MachineId, command.UserId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));
             
            _machineReservationService.ReserveMachineForUser(machines, EmplooyeeRank.Employee,
                machineToReserve, reservation);


            await _allMachines.UpdateAsync(machineToReserve);


        }
    }
}
