//using ProductionScheduler.Application.Abstractions;
//using ProductionScheduler.Application.Exceptions;
//using ProductionScheduler.Core.Entities;
//using ProductionScheduler.Core.Repositories;
//using ProductionScheduler.Core.ValueObjects;
//#refactor usunac calosc po testach 
//namespace ProductionScheduler.Application.Commands.Handlers
//{
//    public class ChangeReservationEmployeeNameHandler : DataChangeHandlerBase, ICommandHandler<ChangeReservationEmployeeName>
//    {
//        private readonly IMachinesRepository _repository;

//        public ChangeReservationEmployeeNameHandler(IMachinesRepository repository)
//        {
//            _repository = repository;
//        }
//        public async Task HandleAsync(ChangeReservationEmployeeName command)
//        {
//            var machine = await GetMachineByReservationIdAsync(_repository, command.ReservationId);


//            if (machine is null)
//            {
//                throw new MachineNotFoundException(command.ReservationId);
//            }

//            var reservationId = new ReservationId(command.ReservationId);
//            var reservation = machine.Reservations
//                .OfType<ReservationForUser>()
//                .SingleOrDefault(x => x.Id == reservationId);


//            if (reservation is null)
//            {
//                throw new ReservationNotFoundException(command.ReservationId);
//            }

//            reservation.ChangeEmployeeName(command.EmployeeName);
//            await _repository.UpdateAsync(machine);

//        }


//    }
//}
