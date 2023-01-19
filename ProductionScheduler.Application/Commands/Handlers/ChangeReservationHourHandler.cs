using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class ChangeReservationHourHandler : DataChangeHandlerBase, ICommandHandler<ChangeReservationHour>
    {
        private readonly IMachinesRepository _repository;

        public ChangeReservationHourHandler(IMachinesRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(ChangeReservationHour command)
        {
            var machine = await GetMachineByReservationIdAsync(_repository, command.ReservationId);

            if (machine is null)
            {
                throw new MachineNotFoundException(command.ReservationId);
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = machine.Reservations
                .OfType<MachineReservation>()
                .SingleOrDefault(x => x.Id == reservationId);


            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            reservation.ChangeHourOfReservation(new Hour(command.Hour));

            await _repository.UpdateAsync(machine);

        }


    }
}
