﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("machines")]
    public class ReservationController : BaseController
    {
        private readonly ICommandHandler<ReserveMachineForEmployee> _reserveForEmployeeHandler;
        private readonly ICommandHandler<ReserveMachineForService> _reserveForServiceHandler;
        private readonly ICommandHandler<ChangeReservationDate> _changeReservationDateHandler;
        private readonly ICommandHandler<ChangeReservationHour> _changeReservationTimeHandler;
        private readonly ICommandHandler<DeleteReservation> _deleteReservationHandler;
        private readonly IMemoryCache _memoryCache;
        public ILogger<HomeController> _logger { get; }
        public ReservationController(ICommandHandler<ReserveMachineForEmployee> reserveForEmployee,
                            ICommandHandler<ReserveMachineForService> reserveForService,
                            ICommandHandler<ChangeReservationDate> changeReservationDate,
                            ICommandHandler<ChangeReservationHour> changeReservationHour, 
                            ICommandHandler<DeleteReservation> deleteReservationHandler, IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            _reserveForEmployeeHandler = reserveForEmployee;
            _reserveForServiceHandler = reserveForService;
            _changeReservationDateHandler = changeReservationDate;
            _changeReservationTimeHandler = changeReservationHour;
            _deleteReservationHandler = deleteReservationHandler;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpPost("{machineId:guid}/reservations/employee")]
        [SwaggerOperation("Create reservation for employee")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateReservationForEmployee(Guid machineId, ReserveMachineForEmployeeDto dto)
        {
            var command = new ReserveMachineForEmployee(dto.MachineId, Guid.NewGuid(), dto.UserId, dto.Date, dto.Hour, dto.employeeName);

            await _reserveForEmployeeHandler.HandleAsync(command with
            {
                ReservationId = Guid.NewGuid(),
                MachineId = machineId,
                UserId = dto.UserId,
                EmployeeName = command.EmployeeName,
            });
            ClearCache();
            return NoContent();
        }

        [HttpPost("{machineId:guid}/reservations/service")]
        [SwaggerOperation("Create reservation for service")]
        [Authorize(Policy = "is-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateReservationForService(ReserveMachineForService command)
        {
            await _reserveForServiceHandler.HandleAsync(command);
            ClearCache();
            return NoContent();
        }

        [HttpPut("reservations/{reservationId:guid}")]
        [SwaggerOperation("Edit reservation date and hour by id")]
        [Authorize(Policy = "is-manager-or-admin")]  
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> EditReservationTime(Guid reservationId, ChangeReservationTimeDto dto)
        {
            var userId = Guid.Parse(HttpContext.User.Identity?.Name);

            var commandHour = new ChangeReservationHour(reservationId, dto.Hour);
            await _changeReservationTimeHandler.HandleAsync(commandHour with { ReservationId = reservationId });

            var commandDate = new ChangeReservationDate(reservationId, dto.Date);
            await _changeReservationDateHandler.HandleAsync(commandDate with { ReservationId = reservationId });
            ClearCache();
            return NoContent();
        }

        [HttpDelete("reservations/{reservationId:guid}")]
        [SwaggerOperation("Delete reservation by id")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteReservation(Guid reservationId)
        {
            var userIdentityId = Guid.Parse(HttpContext.User.Identity?.Name);
            var userRole = HttpContext.User.IsInRole("user") ? "user" : (HttpContext.User.IsInRole("manager") ? "manager" : "admin");

            await _deleteReservationHandler.HandleAsync(new DeleteReservation(reservationId, userIdentityId, userRole));
            ClearCache();
            return NoContent();
        } 
        internal void ClearCache()
        {
            _memoryCache.Remove("machines");
            _logger.LogInformation("Machines cleared from cache");
        }
    }
}
