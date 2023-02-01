using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using System.Web.Http.Cors;


namespace ProductionScheduler.Api.Controllers;

[ApiController] // to odpowiada za to ze nie trzeba uzywac ciagle from body - oznaczasz ze to jest tyylko api
[Route("machines")]
public class MachinesController : ControllerBase
{
    private readonly ICommandHandler<ReserveMachineForEmployee> _reserveForEmployeeHandler;
    private readonly ICommandHandler<ReserveMachineForService> _reserveForServiceHandler;
    private readonly ICommandHandler<ChangeReservationDate> _changeReservationDateHandler;
    private readonly ICommandHandler<ChangeReservationHour> _changeReservationHourHandler;
    private readonly ICommandHandler<ChangeReservationEmployeeName> _changeReservationEmployeeNameHandler;
    private readonly ICommandHandler<DeleteReservation> _deleteReservationHandler;
    private readonly IQueryHandler<GetMachines, IEnumerable<MachineDto>> _getMachinesHandler;


    public MachinesController(ICommandHandler<ReserveMachineForEmployee> reserveForEmployee,
                              ICommandHandler<ReserveMachineForService> reserveForService,
                              ICommandHandler<ChangeReservationDate> changeReservationDate,
                              ICommandHandler<ChangeReservationHour> changeReservationHour,
                              ICommandHandler<ChangeReservationEmployeeName> changeReservationEmployeeName,
                              IQueryHandler<GetMachines, IEnumerable<MachineDto>> getMachines, 
                              ICommandHandler<DeleteReservation> deleteReservationHandler)
    {
        _reserveForEmployeeHandler = reserveForEmployee;
        _reserveForServiceHandler = reserveForService;
        _changeReservationDateHandler = changeReservationDate;
        _changeReservationHourHandler = changeReservationHour;
        _changeReservationEmployeeNameHandler = changeReservationEmployeeName;
        _getMachinesHandler = getMachines;
        _deleteReservationHandler = deleteReservationHandler;
    }

    [HttpGet]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public async Task<ActionResult<IEnumerable<MachineDto>>> Get([FromQuery] GetMachines query) //
        => Ok(await _getMachinesHandler.HandleAsync(query));
 

    //[HttpGet("{id:guid}")] //#refactor
    //public async Task<ActionResult<Reservation>> Get(Guid id)
    //{
    //    var reservation = await _service.GetAsync(id);
    //    if (reservation is null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(reservation);
    //}
    //{machineId:guid}/reservations
    [HttpPost("{machineId:guid}/reservations/employee")]
    public async Task<ActionResult> Post(Guid machineId, ReserveMachineForEmployee command)// #refactor name
    {
        await _reserveForEmployeeHandler.HandleAsync(command with
        {
            ReservationId = Guid.NewGuid(),
            MachineId = machineId
        });

        return CreatedAtAction(nameof(Get), new { command.ReservationId }, null);

    }
    [HttpPost("{machineId:guid}/reservations/service")]
    public async Task<ActionResult> CreateReservationForService(ReserveMachineForService command) // #refactor name
    {
        await _reserveForServiceHandler.HandleAsync(command);
        return NoContent();

    }


    [HttpPut("reservations/{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationHour command) // #refactor
    {
        await _changeReservationHourHandler.HandleAsync(command with { ReservationId = id });

        return NoContent();

    }
    [HttpDelete("reservations/{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _deleteReservationHandler.HandleAsync(new DeleteReservation(id));
        return NoContent();

    }
}
