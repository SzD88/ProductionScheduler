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
        { 
            var reservations = await _allMachines.GetAllAsync();

            return reservations
                .SelectMany(x => x.Reservations) 
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    MachineId = x.MachineId, 
                    EmployeeName = x is ReservationForUser y ? y.EmployeeName : String.Empty,
                    Date = x.Date.Value.Date,
                    Hour = x.Hour
                });  
        } 
        public async Task<Guid?> ReserveForEmployeeAsync(ReserveMachineForEmployee command) 
        {
            var machineId = new MachineId(command.MachineId);
            var allReservations = await _allMachines.GetAllAsync();
             
            var timeforward = new ReservationTimeForward(_clock.Current());
             
             var machines = (await _allMachines.GetAllAsync()).ToList();

            var machineToReserve = machines.SingleOrDefault(x => x.Id == machineId); 
            if ( machineToReserve is   null)
            { 
                return default;
            } 
            var reservation = new ReservationForUser(command.ReservationId, command.MachineId, command.UserId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            _machineReservationService.ReserveMachineForUser(machines, EmplooyeeRank.Employee,
                machineToReserve, reservation);
             
            await _allMachines.UpdateAsync(machineToReserve);
            return reservation.Id;
        } 
        public Task<bool> ChangeReservationDateAsync(ChangeReservationDate command)
        { 
            throw new NotImplementedException(); 
        } 
        public async Task<bool> ChangeReservationHourAsync(ChangeReservationHour command)
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
            if (existingReservation.Date.Value.Date == _clock.Current().Date)
            {
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

        public Task ReserveAllMachinesForServiceAsync(ReserveMachineForService command)
        {
            throw new NotImplementedException(); // #refactor
        }
    }
}
