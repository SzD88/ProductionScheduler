using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Route("machines")]
    public class ReservationController : BaseController
    {
        private readonly ICommandHandler<ReserveMachineForEmployee> _reserveForEmployeeHandler;
        private readonly ICommandHandler<ReserveMachineForService> _reserveForServiceHandler;
        private readonly ICommandHandler<ChangeReservationDate> _changeReservationDateHandler;
        private readonly ICommandHandler<ChangeReservationHour> _changeReservationTimeHandler;
        private readonly ICommandHandler<ChangeReservationEmployeeName> _changeReservationEmployeeNameHandler;
        private readonly ICommandHandler<DeleteReservation> _deleteReservationHandler;
        public ReservationController(ICommandHandler<ReserveMachineForEmployee> reserveForEmployee,
                            ICommandHandler<ReserveMachineForService> reserveForService,
                            ICommandHandler<ChangeReservationDate> changeReservationDate,
                            ICommandHandler<ChangeReservationHour> changeReservationHour,
                            ICommandHandler<ChangeReservationEmployeeName> changeReservationEmployeeName,
                            ICommandHandler<DeleteReservation> deleteReservationHandler)
        {
            _reserveForEmployeeHandler = reserveForEmployee;
            _reserveForServiceHandler = reserveForService;
            _changeReservationDateHandler = changeReservationDate;
            _changeReservationTimeHandler = changeReservationHour;
            _changeReservationEmployeeNameHandler = changeReservationEmployeeName;
            _deleteReservationHandler = deleteReservationHandler;
        }

        [HttpPost("{machineId:guid}/reservations/employee")]
        [SwaggerOperation("Create reservation for employee")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateReservationForEmployee(Guid machineId, ReserveMachineForEmployee command)
        {
            await _reserveForEmployeeHandler.HandleAsync(command with
            {
                ReservationId = Guid.NewGuid(),
                MachineId = machineId
            });
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
            return NoContent();
        }

        [HttpPut("reservations/{reservationId:guid}")]
        [SwaggerOperation("Edit reservation by id")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> EditReservationTime(Guid reservationId, ChangeReservationDateAndTimeDto dto)
        {
            var userId = Guid.Parse(HttpContext.User.Identity?.Name);
             
            if (!HttpContext.User.IsInRole("admin"))
                return Forbid();
            
            var command = new ChangeReservationHour(reservationId, userId, dto.Date, dto.Hour);
             
            await _changeReservationTimeHandler.HandleAsync(command with { ReservationId = reservationId });

            return NoContent();
        }

        [HttpDelete("reservations/{reservationId:guid}")]
        [SwaggerOperation("Delete reservation by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteReservation(Guid reservationId, Guid userId)
        {
            var userIdentityId = Guid.Parse(HttpContext.User.Identity?.Name);
            var userIdentityRole = HttpContext.User.IsInRole("user");

            if (userIdentityId != userId && userIdentityRole)
            {
                return Forbid();
            }
            await _deleteReservationHandler.HandleAsync(new DeleteReservation(reservationId));
            return NoContent();
        }
    }
}
