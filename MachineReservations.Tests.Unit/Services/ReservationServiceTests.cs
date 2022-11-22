using MachineReservations;
using MachineReservations.Api.Commands;
using MachineReservations.Api.Entities;
using MachineReservations.Api.Services;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Repositories;
using MachineReservations.Tests.Unit.Shared;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace SDMySpot.Tests.Unit.Services
{
    public class ReservationServiceTests
    {

        [Fact]
        public void given_reservation_for_not_taken_date_create_reservation_should_succeed()
        {
            var command = new CreateReservation(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Guid.NewGuid(),
                DateTime.UtcNow.AddDays(2),
                "Szop",
                12);

            //Guid MachineId, 
            // Guid ReservationId,
            // DateTime Date, 
            //  string EmployeeName,
            // short Hour

            var reservationId = _reservationService.Create(command);

            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }
        #region arrange
        private readonly IClock _clock = new TestClock(); 
        private readonly IPeriodMachineReservationRepository _repository;
        private readonly IReservationService _reservationService;
        private readonly List<PeriodMachineReservation> _periodMachineReservation;
        public ReservationServiceTests()
        {
            /// add all machine spots
            _repository = new InMemoryPeriodMachineReservationRepository(_clock);
            _periodMachineReservation = new List<PeriodMachineReservation>()
             {
             new  (Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(_clock.Current()), "P1"),
             new  (Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(_clock.Current()), "P2"),
             new  (Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(_clock.Current()), "P3"),
             new  (Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(_clock.Current()), "P4"),
             new  (Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(_clock.Current()), "P5")

             };
            _reservationService = new ReservationService(_clock, _repository);
        }
        #endregion
    }
}
