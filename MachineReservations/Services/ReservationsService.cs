using MachineReservations.Api.Commands;
using MachineReservations.Api.Controllers.Models;
using MachineReservations.Api.DTO;
using MachineReservations.Api.Entities;
using MachineReservations.Api.Exceptions;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Core.ValueObjects;

namespace MachineReservations.Api.Services
{
    public class ReservationsService : IReservationService
    {
        private readonly IClock _clock;
        private readonly IEnumerable< WeeklyMachineReservation> _weeklyMachineReservations;

        public ReservationsService(IClock clock, IEnumerable<WeeklyMachineReservation> weeklyMachineReservations)
        {
            _clock = clock; 
            _weeklyMachineReservations = weeklyMachineReservations;
        }
       
        public ReservationDto Get(Guid id)
       => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public IEnumerable<ReservationDto> GetAllWeekly()
            => _weeklyMachineReservations.SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                MachineId = x.MachineId,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date,
                Hour = x.Hour
            }); // ?? w okolicahc 13 odcinka 



        public Guid? Create(CreateReservation command) // nullable daje to ze jak sie nie uda zwrocisz null
        // i bedziesz mogl to wykorzystać 
        {
            var machineId = new MachineId(command.MachineId);
            var weeklyMachineReservation = _weeklyMachineReservations
                .SingleOrDefault(x => x.Id == machineId);
            if (weeklyMachineReservation == null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.MachineId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            var curr = _clock.Current();

            //przekazujesz rezerwacje i czas obecny 
            weeklyMachineReservation.AddReservation(reservation, new Date(_clock.Current()));
            return reservation.Id;
        }

        public bool Update(ChangeReservationHour command)
        {
            var weeklyMachineReservation = GetWeeklyMachineReservationByReservation(command.ReservationId);

            if (weeklyMachineReservation is null) 
                return false; 

            var reservationId = new ReservationId(command.ReservationId);

            var existingReservation = weeklyMachineReservation.Reservations
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
            return true;
        }
        public bool Delete(DeleteReservation command)
        {
            var weeklyMachineReservation = GetWeeklyMachineReservationByReservation
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

            return true;
        }

        private WeeklyMachineReservation GetWeeklyMachineReservationByReservation(ReservationId reservationId) //
            => _weeklyMachineReservations.SingleOrDefault(x => x.Reservations.Any
            (r => r.Id == reservationId));
    }
}
