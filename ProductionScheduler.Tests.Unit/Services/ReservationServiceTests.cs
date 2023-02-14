//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using MachineReservations.Tests.Unit.Shared;
//using ProductionScheduler.Application.Commands;
//using ProductionScheduler.Application.Services;
//using ProductionScheduler.Core.Abstractions;
//using ProductionScheduler.Core.DomainServices;
//using ProductionScheduler.Core.Policies;
//using ProductionScheduler.Core.Repositories;
//using ProductionScheduler.Core.ValueObjects;
//using ProductionScheduler.Infrastructure.DAL.Repositories;
//using Shouldly;
//using Xunit;

//namespace MachineReservations.Tests.Unit.Services
//{
//    public class ReservationServiceTests
//    {

//        [Fact] // exxpanded #18 - 1:01:01
//        public async Task given_reservation_for_not_taken_date_create_reservation_should_succeed()
//        {
//            var command = new ReserveMachineForEmployee(
//                Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                Guid.NewGuid(),
//                new DateTime(2023, 01, 18), // tutaj 
//                "Szop",
//                12);

//            //Guid MachineId, 
//            // Guid ReservationId,
//            // DateTime Date, 
//            //  string EmployeeName,


//            var reservationId = await _reservationService.ReserveForEmployeeAsync(command);

//            reservationId.ShouldNotBeNull();
//            reservationId.Value.ShouldBe(command.ReservationId);
//        }

//        [Fact] // exxpanded #18 - 1:01:01
//        public async Task test2()
//        {
//            var periodReservations = (await _repository.GetAllAsync()).First();
//            var command = new ReserveMachineForEmployee( periodReservations.Id, Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Szoopa",  13);

//            var reservationId = await _reservationService.ReserveForEmployeeAsync(command);

//            //assert 
//            reservationId.ShouldNotBeNull();
//            reservationId.Value.ShouldBe(command.ReservationId); 
        
        
//        }
//        #region arrange
//        private readonly IClock _clock;
//        private readonly IMachinesRepository _repository;
//        private readonly IReservationService _reservationService;
//        public ReservationServiceTests()
//        {
//            /// add all machine spots
//            _clock = new TestClock();
//             _repository = new InMemoryPeriodMachineReservationRepository(_clock);

//            var machineReservationService = new MachineReservationService(
//              new IReservationPolicy[]
//            {
//            new EmployeeReservationPolicy(_clock),
//            new ManagerReservationPolicy(_clock),
//            new AdminReservationPolicy(_clock)

//            }, _clock); 

//            _reservationService = new ReservationService(_clock, _repository, machineReservationService);
//        }
//        #endregion
//    }
//}
