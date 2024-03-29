﻿using ProductionScheduler.Application.DTO;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.DAL.Handlers
{
    internal static class Extensions
    {
        public static MachineDto AsDto(this Machine entity)
        {
            var asDto = new MachineDto
            {
                Id = entity.Id.Value.ToString(),
                Name = entity.Name,
                From = entity.TimeForward.From.Value.DateTime,
                To = entity.TimeForward.To.Value.DateTime,
                Reservations = entity.Reservations.Select(x => new ReservationDto
                {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    EmployeeName = x is ReservationForUser y ? y.EmployeeName : "service",
                    Type = x is ReservationForUser ? "machine" : "service",  
                    Date = x.Date.Value.Date,
                    Hour = x.Hour.Value
                })
            };
            return asDto;
        }
        public static UserDto AsDto(this User entity)
       => new()
       {
           Id = entity.Id,
           UserName = entity.UserName,
           FullName = entity.FullName
       };
    }
}

