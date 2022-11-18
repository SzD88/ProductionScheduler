﻿using MachineReservations.Api.Commands;
using MachineReservations.Api.Controllers.Models;
using MachineReservations.Api.DTO;
using MachineReservations.Api.Entities;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Core.ValueObjects;

namespace MachineReservations.Api.Services
{
    public class ReservationsServices
    {
        private static Clock Clock = new();

        private static readonly List<WeeklyMachineReservation> WeeklyMachineReservations = new()
        {
            new WeeklyMachineReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(Clock.Current()), "P1"),
            new WeeklyMachineReservation(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(Clock.Current()), "P2"),
            new WeeklyMachineReservation(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(Clock.Current()), "P3"),
            new WeeklyMachineReservation(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(Clock.Current()), "P4"),
            new WeeklyMachineReservation(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(Clock.Current()), "P5")

        };
        public ReservationDto Get(Guid id)
       => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public IEnumerable<ReservationDto> GetAllWeekly()
            => WeeklyMachineReservations.SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                MachineId = x.MachineId,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            }); // ?? w okolicahc 13 odcinka 



        public Guid? Create(CreateReservation command) // nullable daje to ze jak sie nie uda zwrocisz null
        // i bedziesz mogl to wykorzystać 
        {
            var machineId = new MachineId(command.MachineId);
            var weeklyMachineSpot = WeeklyMachineReservations
                .SingleOrDefault(x => x.Id == machineId);
            if (weeklyMachineSpot == null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.MachineId,
                command.EmployeeName, command.Hour, new Date(command.Date));

            weeklyMachineSpot.AddReservation(reservation, new Date(Clock.Current()));
            return reservation.Id;
        }

        public bool Update(ChangeReservationHour command)
        {
            var weeklyMachineReservation = GetWeeklyMachineReservationByReservation(command.ReservationId);

            if (weeklyMachineReservation is null)
            {
                return false;
            }
            var reservationId = new ReservationId(command.ReservationId);
            var existingReservation = weeklyMachineReservation.Reservations
                .SingleOrDefault(x => x.Id == reservationId);
            if (existingReservation is null)
            {
                return false;
            }
            if (existingReservation.Date.Value.Date <= Clock.Current())
            {
                return false;
            }
            existingReservation.ChangeHourOfReservation(command.Hour);
            return true;
        }
        public bool Delete(DeleteReservation command)
        {
            var weeklyMachineReservation = GetWeeklyMachineReservationByReservation
               (command.ReservationId);
            if (weeklyMachineReservation is null)
            {
                return false;
            }
            var reservationId = new ReservationId(command.ReservationId);
            var existingReservation = weeklyMachineReservation.Reservations
                .SingleOrDefault(x => x.Id == reservationId);

            if (existingReservation is null)
            {
                return false;
            }
            weeklyMachineReservation.RemoveReservation(command.ReservationId);

            return true;
        }

        private WeeklyMachineReservation GetWeeklyMachineReservationByReservation(ReservationId reservationId) //
            => WeeklyMachineReservations.SingleOrDefault(x => x.Reservations.Any
            (r => r.Id == reservationId));
    }
}
