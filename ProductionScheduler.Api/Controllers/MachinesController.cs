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
    [HttpGet]
   // [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieves all machines")]
    public async Task<ActionResult<IEnumerable<MachineDto>>> Get([FromQuery] GetMachines query)
        => OkOrNotFound(await _getMachinesHandler.HandleAsync(query));

}
