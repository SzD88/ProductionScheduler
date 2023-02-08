using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
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
        private readonly ICommandHandler<ChangeReservationHour> _changeReservationHourHandler;
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
            _changeReservationHourHandler = changeReservationHour;
            _changeReservationEmployeeNameHandler = changeReservationEmployeeName;
            _deleteReservationHandler = deleteReservationHandler;
        }

        [HttpPost("{machineId:guid}/reservations/employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(Guid machineId, ReserveMachineForEmployee command)// #refactor name
        {
            await _reserveForEmployeeHandler.HandleAsync(command with
            {
                ReservationId = Guid.NewGuid(),
                MachineId = machineId
            }); 
            return NoContent(); 
        }


        [HttpPost("{machineId:guid}/reservations/service")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateReservationForService(ReserveMachineForService command) // #refactor name
        {
            await _reserveForServiceHandler.HandleAsync(command);
            return NoContent(); 
        }
       

        [HttpPut("reservations/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(Guid id, ChangeReservationHour command) // #refactor
        {
            await _changeReservationHourHandler.HandleAsync(command with { ReservationId = id });

            return NoContent(); 
        }

        [HttpDelete("reservations/{id:guid}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _deleteReservationHandler.HandleAsync(new DeleteReservation(id));
            return NoContent(); 
        }
    } 
}
