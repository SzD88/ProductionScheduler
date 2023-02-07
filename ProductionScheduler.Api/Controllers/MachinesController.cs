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


    public MachinesController(IQueryHandler<GetMachines, IEnumerable<MachineDto>> getMachines,
    {

        _getMachinesHandler = getMachines;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineDto>>> Get([FromQuery] GetMachines query)
        => Ok(await _getMachinesHandler.HandleAsync(query));

}
