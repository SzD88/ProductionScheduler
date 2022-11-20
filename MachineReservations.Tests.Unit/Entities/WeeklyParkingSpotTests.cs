using MySpot.Api.Controllers.Models;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;
using Shouldly;
using System;
using Xunit;

namespace SDMySpot.Tests.Unit.Entities
{
    public class WeeklyParkingSpotTests
    {   // snake case
        //arrange act assert

        // [Fact] // for simple use, just 1 test for 1 action
        [Theory] // now you can work on variables
        [InlineData("2022-10-08")]
        [InlineData("2022-12-08")]
        public void given_invalid_date_add_reservation_should_fail(string dateString)// changed too
        {
            //arrange
            var invalidDate = DateTime.Parse(dateString);
            var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id,
                "JohnSzop", "TabRej", new Date(invalidDate));
            //act
            //sprawdza czy wywala exception zeby nie wywalilo exceptiona a zeby miec boola
            var exception = Record.Exception(() => _weeklyParkingSpot
            .AddReservation(reservation, _now));

            //assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidReservationDateException>(exception);
            //effect as above we can get with nuget package "Shouldly" 
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidReservationDateException>();
        }

        [Fact]
        public void given_reservation_for_alredy_existing_date_add_reservation_should_fail()
        {
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "JohnSzp",
                "Pl1234", reservationDate);
            var nextReservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "JohnSzp",
                "Pl1234", reservationDate);
            //create reservation because we are checking if override it throw exception 
            _weeklyParkingSpot.AddReservation(reservation, _now);

            var exception = Record.Exception(() => _weeklyParkingSpot
           .AddReservation(nextReservation, _now));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ParkingSpotAlredyReservedException>();
        }
        [Fact]
        public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
        {
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "JohnSzp",
                "Pl1234", reservationDate);

            _weeklyParkingSpot.AddReservation(reservation, _now);

            _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
        }

        #region
        private readonly WeeklyParkingSpot _weeklyParkingSpot;
        private readonly Date _now;
        public WeeklyParkingSpotTests()
        {
            //arrange on ctor lvl
            _now = new Date(new DateTime(2022, 11, 07));
            _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");

        }
        #endregion
    }
}
