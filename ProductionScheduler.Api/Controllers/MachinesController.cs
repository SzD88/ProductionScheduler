using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers;

[ApiController]
[Route("machines")]
public class MachinesController : BaseController
{
    private readonly IQueryHandler<GetMachines, IEnumerable<MachineDto>> _getMachinesHandler;

    public MachinesController(IQueryHandler<GetMachines, IEnumerable<MachineDto>> getMachines)
    {
        _getMachinesHandler = getMachines;
    }

    [HttpPost("/test")]
    public async Task<ActionResult<testQ>> Test([FromQuery] testQ enter)
    {
       await Task.Delay(1000);
        return Ok(enter);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieves all machines")]
    public async Task<ActionResult<IEnumerable<MachineDto>>> Get([FromQuery] GetMachines query)
        => OkOrNotFound(await _getMachinesHandler.HandleAsync(query));

}
