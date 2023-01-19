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
            // klasa ma enkapsulowac logike dla danej czynnosci 


            var machineId = new MachineId(command.MachineId);
            var allReservations = await _allMachines.GetAllAsync();

            // # i musisz to zrobic wczesniej jezeli chcesz za kursem podarzac niestety 
            //##REFACTOR - to jest do poprawy, ty masz miec zawsze sztywny termin max rezerwacji maszyny systemowy - to ma byc ten time forward np 28 dni albo 7
            // tam nie ma byc godziny, ten time forward zawsze ma wynosic sztywny czas od clock current - wtedy dopiero to bedzie mialo sens ponizej czyli get by period
            // to trzeba doprowadzic do dzialania w kolejnosci zeby miec porzadek i moc jej uzywac 
            var timeforward = new ReservationTimeForward(_clock.Current());

            // lista rezerwacji we wskazanym okresie czasu, jezeli nie ma takowych to co mi szkodzi dodac?
            // zwroci taki ktory jest powiazany z istniejacymi rezerwacjami oraz wyznaczonym czasem forward
            // ale po co to , jak nie ma rezerwacji to zwroci puste 


            //# tu jest problem #refactor 
            //   var machines = (await _allMachines.GetByPeriodAsync(timeforward)).ToList();
            var machines = (await _allMachines.GetAllAsync()).ToList();

            var machineToReserve = machines.SingleOrDefault(x => x.Id == machineId);
            if (machineToReserve is null)
            {
              throw new  MachineNotFoundException(machineId);
            }

            var reservation = new MachineReservation(command.ReservationId, command.MachineId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            //#refactor hardcoded manager
            _machineReservationService.ReserveMachineForUser(machines, EmplooyeeRank.Employee,
                machineToReserve, reservation);

            //przekazujesz rezerwacje i czas obecny 
            // machineToReserve.AddReservation(reservation, new Date(_clock.Current()));
            await _allMachines.UpdateAsync(machineToReserve);

            // no return if everything was fine controller is going to continue
        }
    }
}
