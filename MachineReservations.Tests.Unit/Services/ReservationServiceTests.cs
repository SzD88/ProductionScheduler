using MachineReservations;
using MachineReservations.Api.Commands;
using MachineReservations.Api.Entities;
using MachineReservations.Api.Services;
using Shouldly;
using System;
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
       
            var reservationId =   _reservationServie.Create(command);

            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }
        #region arrange
        private readonly IReservationService _reservationServie;
        private readonly IClock _clock;
        private readonly List<WeeklyMachineReservation> _weeklyMachineReservation;
        public ReservationServiceTests()
        {
            /// add all machine spots
            _weeklyMachineReservation
        }
        #endregion
    }
}
