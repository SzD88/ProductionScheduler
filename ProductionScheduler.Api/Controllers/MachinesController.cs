using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    public ILogger<MachinesController> _logger { get; }
    public MachinesController(IQueryHandler<GetMachines, IEnumerable<MachineDto>> getMachines, IMemoryCache memoryCache, ILogger<MachinesController> logger)
    {
        _getMachinesHandler = getMachines;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Retrieves all machines")]
    public async Task<ActionResult<IEnumerable<MachineDto>>> Get([FromQuery] GetMachines query)
    {
        var machines = _memoryCache.Get<IEnumerable<MachineDto>>("machines");
        if (machines == null)
        {
            _logger.LogInformation("Fetching from service.");
            machines = await _getMachinesHandler.HandleAsync(query);
            _memoryCache.Set("machines", machines, TimeSpan.FromMinutes(5));
        }
        else
        {
            _logger.LogInformation("Fetching from cache.");
        }
        return OkOrNotFound(machines);
    }
}
