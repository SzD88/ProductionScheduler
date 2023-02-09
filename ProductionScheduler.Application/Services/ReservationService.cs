using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Core.DomainServices;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Exceptions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Core.Abstractions;

namespace ProductionScheduler.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IClock _clock;
        private readonly IMachinesRepository _allMachines;
        private readonly IMachineReservationService _machineReservationService;


        public ReservationService(IClock clock, IMachinesRepository repository,
            IMachineReservationService machineReservationService)
        {
            _clock = clock;
            _allMachines = repository;
            _machineReservationService = machineReservationService;
        }

        public async Task<ReservationDto> GetAsync(Guid id)
        {
            var reservations = await GetAllAsync();

            return reservations.SingleOrDefault(x => x.Id == id);

        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {//#refactor get all or get all by time period?
            var reservations = await _allMachines.GetAllAsync();

            return reservations
                .SelectMany(x => x.Reservations)
//                .OfType<MachineReservation>() // it is correct but what about Servicing machines? 
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    // jezeli x jest typu machinereservation przypisz go do y jako typ pochodny 
                    EmployeeName = x is ReservationForUser y ? y.EmployeeName : String.Empty,
                    Date = x.Date.Value.Date,
                    Hour = x.Hour
                }); // ?? w okolicahc 13 odcinka 
        }


        public async Task<Guid?> ReserveForEmployeeAsync(ReserveMachineForEmployee command) // nullable daje to ze jak sie nie uda zwrocisz null
        // i bedziesz mogl to wykorzystać 
        {
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
            if ( machineToReserve is   null)
            { 
                return default;
            }

            var reservation = new ReservationForUser(command.ReservationId, command.MachineId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            //#refactor hardcoded manager
            _machineReservationService.ReserveMachineForUser(machines, EmplooyeeRank.Employee,
                machineToReserve, reservation);

            //przekazujesz rezerwacje i czas obecny 
            // machineToReserve.AddReservation(reservation, new Date(_clock.Current()));
            await _allMachines.UpdateAsync(machineToReserve);
            return reservation.Id;
        }

        public async Task ReserveAllMachinesForServiceAsync(ReserveMachineForService command)
        {
           
        }

        public Task<bool> ChangeReservationDateAsync(ChangeReservationDate command)
        { 
            throw new NotImplementedException(); 
        }

        public async Task<bool> ChangeReservationHourAsync(ChangeReservationTime command)
        {
            var machineToReserve = await GetMachineByReservationIdAsync(command.ReservationId);

            if (machineToReserve is null)
                return false;

            var reservationId = new ReservationId(command.ReservationId);

            var existingReservation = machineToReserve.Reservations
                .SingleOrDefault(x => x.Id == reservationId);
            if (existingReservation is null)
            {
                return false;
            }
            var clockHour = _clock.Current().Hour;
            var existingReservationHour = existingReservation.Hour;
            // ty chcesz sprawdzic czy  godzina rezerwacji jest pozniej niz obecnie rozpoczeta godzina

            // if it is today 
            if (existingReservation.Date.Value.Date == _clock.Current().Date)
            {
                //check if reservation hour is after current hour
                if (existingReservation.Hour.Value <= _clock.Current().Hour)
                {
                    throw new InvalidTimeOfReservation();
                }
            }

            existingReservation.ChangeHourOfReservation(command.Hour);
            await _allMachines.UpdateAsync(machineToReserve);
            return true;
        }
        public async Task<bool> DeleteAsync(DeleteReservation command)
        {
            var weeklyMachineReservation = await GetMachineByReservationIdAsync
               (command.ReservationId);
            if (weeklyMachineReservation is null)
            {
                return false;
            }
            var reservationId = new ReservationId(command.ReservationId);
            var existingReservation = weeklyMachineReservation.Reservations
                .SingleOrDefault(x => x.Id == reservationId);

            if (existingReservation is null)
            {
                return false;
            }
            weeklyMachineReservation.RemoveReservation(command.ReservationId);
            await _allMachines.DeleteAsync(weeklyMachineReservation);
            return true;
        }

        private async Task<Machine> GetMachineByReservationIdAsync(ReservationId reservationId)  

        {
            var periodMachineReservations = await _allMachines.GetAllAsync();

            return periodMachineReservations.SingleOrDefault(x => x.Reservations.Any
                 (r => r.Id == reservationId));
        }

       
    }
}
