
using MachineReservations.Api.Controllers.Models;
using MachineReservations.Api.Entities;
using MachineReservations.Api.Exceptions;
using MachineReservations.Api.ValueObjects;
using Shouldly;
using System;
using Xunit;

namespace MachineReservations.Tests.Unit.Entities
{
    public class PeriodMachineReservationTest
    {   // snake case
        //arrange act assert

        // [Fact] // for simple use, just 1 test for 1 action
        [Theory] // now you can work on variables
        [InlineData("2022-10-08")]
        public void given_invalid_date_of_single_add_reservation_create_class_should_fail(string dateString)// changed too
        {
            // public Reservation(ReservationId id, MachineId machineId, 
            //  EmployeeName employeeName, Hour hour, Date dateTime)
            //arrange+-
            var invalidDate = DateTime.Parse(dateString);

            var exception0 = Record.Exception(() =>
               new Reservation
                (Guid.NewGuid(),
                _weeklyMachineReservation.Id,
                "JohnSzop",
                13,
                new Date(invalidDate)));
            //act

            exception0.ShouldNotBeNull();
            exception0.ShouldBeOfType<DateFromPastException>();
        }
        [Theory] // now you can work on variables
        [InlineData("2022-10-08")]
        public void given_invalid_date_add_reservation_used_by_service_should_fail(string dateString)// changed too
        {

            var invalidDate = DateTime.Parse(dateString);

            var reservation =
               new Reservation
                (Guid.NewGuid(),
                _weeklyMachineReservation.Id,
                "JohnSzop",
                13,
                new Date(_now.AddDays(1)));

            var exception = Record.Exception(() => _weeklyMachineReservation
             .AddReservation(reservation, _now.AddDays(-2)));

            //assert
            //  Assert.NotNull(exception);
            // Assert.IsType<InvalidHourException>(exception);
            //effect as above we can get with nuget package "Shouldly" 
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DateFromPastException>();
        }

        [Theory]
        [InlineData("2022-11-22")]
        public void given_invalid_hour_of_single_add_reservation_create_class_should_fail(string dateString)// changed too
        {

            var invalidDate = DateTime.Parse(dateString);

            var exception = Record.Exception(() =>
             new Reservation
              (Guid.NewGuid(),
              _weeklyMachineReservation.Id,
              "JohnSzop",
              17,
              new Date(_now.AddDays(1))));


            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidHourException>();
        }


        [Fact]
        public void given_reservation_for_alredy_existing_date_add_reservation_should_fail()
        {
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation
                (Guid.NewGuid(),
                _weeklyMachineReservation.Id,
                "JohnSzop",
                13, // invalid hour
                reservationDate);

            var nextReservation = new Reservation
                (Guid.NewGuid(),
                _weeklyMachineReservation.Id,
                "JohnSzop",
                13, // invalid hour
                reservationDate);

            //create reservation because we are checking if override it throw exception 
            _weeklyMachineReservation.AddReservation(reservation, _now);

            var exception = Record.Exception(() => _weeklyMachineReservation
           .AddReservation(nextReservation, _now));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MachineAlredyReservedException>();
        }
        [Fact]
        public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
        {
            var reservationDate = _now.AddDays(1);
            var reservation = new Reservation
                (Guid.NewGuid(),
                _weeklyMachineReservation.Id,
                "JohnSzp",
                12,
                reservationDate);

            _weeklyMachineReservation.AddReservation(reservation, _now);

            _weeklyMachineReservation.Reservations.ShouldHaveSingleItem();
        }

        #region
        private readonly PeriodMachineReservation _weeklyMachineReservation;
        private readonly Date _now;
        public PeriodMachineReservationTest()
        {
            //arrange on ctor lvl
            _now = new Date(new DateTime(2022, 11, 22));
            _weeklyMachineReservation = new PeriodMachineReservation(
                Guid.NewGuid(),
                new ReservationTimeForward(_now),
                "P1");

        }
        #endregion
    }
}
