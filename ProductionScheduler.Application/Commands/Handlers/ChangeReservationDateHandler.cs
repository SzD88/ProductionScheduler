using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class ChangeReservationDateHandler : DataChangeHandlerBase , ICommandHandler<ChangeReservationDate>
    {
        private readonly IMachinesRepository _repository;

        public ChangeReservationDateHandler(IMachinesRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(ChangeReservationDate command)
        {
            var machine = await  GetMachineByReservationIdAsync( _repository, command.ReservationId );

            if (machine is null)
            {
                throw new MachineNotFoundException(command.ReservationId);
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = machine.Reservations
                .OfType<ReservationForUser>()
                .SingleOrDefault(x => x.Id == reservationId);
             
            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            reservation.ChangeDateOfReservation( new Date(command.Date.Date));
            await _repository.UpdateAsync(machine); 
        }
    }
}
