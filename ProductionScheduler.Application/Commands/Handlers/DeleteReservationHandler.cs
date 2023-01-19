using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class DeleteReservationHandler : DataChangeHandlerBase, ICommandHandler<DeleteReservation>
    {
        private readonly IMachinesRepository _repository;

        public DeleteReservationHandler(IMachinesRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(DeleteReservation command)
        {
            var machine = await GetMachineByReservationIdAsync(_repository, command.ReservationId);

            if (machine is null)
            {
                throw new MachineNotFoundException(command.ReservationId);
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = machine.Reservations
                .SingleOrDefault(x => x.Id == reservationId);


            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            machine.RemoveReservation(reservationId);

            await _repository.UpdateAsync(machine);

        }
    }
}
