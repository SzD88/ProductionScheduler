using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Exceptions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IClock _clock;
        private readonly IPeriodMachineReservationRepository _repository;

        public ReservationService(IClock clock, IPeriodMachineReservationRepository repository)
        {
            _clock = clock;
            _repository = repository;
        }

        public ReservationDto Get(Guid id)
       => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public IEnumerable<ReservationDto> GetAllWeekly()
            => _repository.GetAll().SelectMany(x => x.Reservations)
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
            var periodMachineReservation = _repository.GetAll()
                .SingleOrDefault(x => x.Id == machineId);
            if (periodMachineReservation == null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.MachineId,
                command.EmployeeName, new Hour(command.Hour), new Date(command.Date));

            

            //przekazujesz rezerwacje i czas obecny 
            periodMachineReservation.AddReservation(reservation, new Date(_clock.Current()));
            _repository.Update(periodMachineReservation);
            return reservation.Id;
        }

        public bool Update(ChangeReservationHour command)
        {
            var periodMachineReservation = GetPeriodMachineReservationByReservation(command.ReservationId);

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
            _repository.Update(periodMachineReservation);
            return true;
        }
        public bool Delete(DeleteReservation command)
        {
            var weeklyMachineReservation = GetPeriodMachineReservationByReservation
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
            _repository.Delete(weeklyMachineReservation);
            return true;
        }

        private PeriodMachineReservation GetPeriodMachineReservationByReservation(ReservationId reservationId) //
            => _repository.GetAll().SingleOrDefault(x => x.Reservations.Any
            (r => r.Id == reservationId));
    }
}
