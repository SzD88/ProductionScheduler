using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Application.Commands.Handlers
{
    public class DeleteReservationHandler : DataChangeHandlerBase, ICommandHandler<DeleteReservation>
    {
        private readonly IMachinesRepository _repository;
        //  private readonly Ireserva _reservationsRepository;

        public DeleteReservationHandler(IMachinesRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(DeleteReservation command)
        {
            var reservationId = new ReservationId(command.ReservationId);
            var userId = new UserId(command.UserId);
            var userRole = new Role(command.UserRole);

            var machine = await GetMachineByReservationIdAsync(_repository, command.ReservationId);

            if (machine is null)
            {
                throw new MachineNotFoundException(command.ReservationId);
            }

            var reservation = machine.Reservations
                .SingleOrDefault(x => x.Id == reservationId);

            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }
            var proj = (ReservationForUser)reservation;

            if (userRole == "user" && proj.UserId != userId)
            {
                throw new DeleteReservationNotAllowed();
            } 
            machine.RemoveReservation(reservationId);

            await _repository.UpdateAsync(machine);

        }
    }
}
