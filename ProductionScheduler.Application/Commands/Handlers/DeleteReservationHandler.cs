using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Exceptions;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;

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
            // if role is user - check if reservation id == user name 
            //# tu skonczyles #finish
            // tutaj w repozytorium znajdz rezerwacje 

            //zastanow sie co otrzymujesz z repo i jak to tutaj rozdmuchać 
            var machine = await GetMachineByReservationIdAsync(_repository, command.ReservationId);

            if (machine is null)
            {
                throw new MachineNotFoundException(command.ReservationId);
            }

            var reservation = machine.Reservations
                .SingleOrDefault(x => x.Id == reservationId);

           //  var employeeName = new EmployeeName(reservation.name);


            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            machine.RemoveReservation(reservationId);

            await _repository.UpdateAsync(machine);

        }
    }
}
