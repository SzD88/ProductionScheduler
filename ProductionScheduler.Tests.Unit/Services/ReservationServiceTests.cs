﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MachineReservations.Tests.Unit.Shared;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure.DAL.Repositories;
using Shouldly;
using Xunit;

namespace MachineReservations.Tests.Unit.Services
{
    public class ReservationServiceTests
    {

        [Fact] // exxpanded #18 - 1:01:01
        public async Task given_reservation_for_not_taken_date_create_reservation_should_succeed()
        {
            var command = new CreateReservation(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Guid.NewGuid(),
                new DateTime(2022, 11, 25), // tutaj 
                "Szop",
                12);

            //Guid MachineId, 
            // Guid ReservationId,
            // DateTime Date, 
            //  string EmployeeName,
            // short Hour

            var reservationId = await _reservationService.CreateAsync(command);

            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

        [Fact] // exxpanded #18 - 1:01:01
        public async Task test2()
        {
            var periodReservations = (await _repository.GetAllAsync()).First();
            var command = new CreateReservation( periodReservations.Id, Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Szoopa",  13);

            var reservationId = await _reservationService.CreateAsync(command);

            //assert 
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId); 
        
        
        }
        #region arrange
        private readonly IClock _clock;
        private readonly IPeriodMachineReservationRepository _repository;
        private readonly IReservationService _reservationService;
        public ReservationServiceTests()
        {
            /// add all machine spots
            _clock = new TestClock();
           //  _repository = new InMemoryPeriodMachineReservationRepository(_clock);
            _reservationService = new ReservationService(_clock, _repository);
        }
        #endregion
    }
}