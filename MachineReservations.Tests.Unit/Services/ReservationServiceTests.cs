using MySpot.Api.Commands;
using MySpot.Api.Controllers.Models;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
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
                DateTime.UtcNow.AddMinutes(5),
                "Szop",
                "licPl23");
          var reservationId =   _reservationServie.Create(command);

            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }
        #region arrange
        private readonly ReservationsService _reservationServie;
        public ReservationServiceTests()
        {
            _reservationServie = new ReservationsService();
        }
        #endregion
    }
}
