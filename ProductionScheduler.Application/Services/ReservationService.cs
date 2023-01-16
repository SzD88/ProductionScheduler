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
        private readonly IPeriodMachineReservationRepository _allMachines;
        private readonly IMachineReservationService _machineReservationService;


        public ReservationService(IClock clock, IPeriodMachineReservationRepository repository,
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
        {
            var reservations = await _allMachines.GetAllAsync();

            return reservations
                .SelectMany(x => x.Reservations)
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    EmployeeName = x.EmployeeName,
                    Date = x.Date.Value.Date,
                    Hour = x.Hour
                }); // ?? w okolicahc 13 odcinka 
        }


        public async Task<Guid?> CreateAsync(CreateReservation command) // nullable daje to ze jak sie nie uda zwrocisz null
        // i bedziesz mogl to wykorzystać 
        {
            var machineId = new MachineId(command.MachineId);
            var allReservations = await _allMachines.GetAllAsync();
            var timeforward = new ReservationTimeForward(_clock.Current());

            // lista rezerwacji we wskazanym okresie czasu, jezeli nie ma takowych to co mi szkodzi dodac?
            // zwroci taki ktory jest powiazany z istniejacymi rezerwacjami oraz wyznaczonym czasem forward
            // ale po co to , jak nie ma rezerwacji to zwroci puste 


            //# tu jest problem #refactor 
            // var periodMachineReservations = (await _allMachines.GetByPeriodAsync(timeforward)).ToList();
            var periodMachineReservations = (await _allMachines.GetAllAsync()).ToList();

            var machineToReserve = periodMachineReservations.SingleOrDefault(x => x.Id == machineId);

            // bierze wszystkie okresowe rezerwacje maszyny i 
            //sprawdza czy ktoras ma takie same id maszyny jak podane id maszyny w zapytaniu
            if ( machineToReserve is   null)
            {
                // nie ma takiej maszyny?
                // czy nie ma rezerwacji na takiej maszynie 
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.MachineId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            //#refactor hardcoded manager
            _machineReservationService.ReserveMachineForUser(periodMachineReservations, EmplooyeeRank.Employee,
                machineToReserve, reservation);

            //przekazujesz rezerwacje i czas obecny 
            // machineToReserve.AddReservation(reservation, new Date(_clock.Current()));
            await _allMachines.UpdateAsync(machineToReserve);
            return reservation.Id;
        }

        public async Task<bool> UpdateAsync(ChangeReservationHour command)
        {
            var periodMachineReservation = await GetPeriodMachineReservationByReservationAsync(command.ReservationId);

            if (periodMachineReservation is null)
                return false;

            var reservationId = new ReservationId(command.ReservationId);

            var existingReservation = periodMachineReservation.Reservations
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
            await _allMachines.UpdateAsync(periodMachineReservation);
            return true;
        }
        public async Task<bool> DeleteAsync(DeleteReservation command)
        {
            var weeklyMachineReservation = await GetPeriodMachineReservationByReservationAsync
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

        private async Task<PeriodMachineReservation> GetPeriodMachineReservationByReservationAsync(ReservationId reservationId) //

        {
            var periodMachineReservations = await _allMachines.GetAllAsync();

            return periodMachineReservations.SingleOrDefault(x => x.Reservations.Any
                 (r => r.Id == reservationId));
        }
    }
}
