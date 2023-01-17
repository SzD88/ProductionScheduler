
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Api.Controllers;

[ApiController] // to odpowiada za to ze nie trzeba uzywac ciagle from body - oznaczasz ze to jest tyylko api
[Route(template: "reservations")]
public class ReservationsController : ControllerBase
{


    private readonly IReservationService _service;

    public ReservationsController(IReservationService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task< ActionResult<IEnumerable<Reservation>>> Get() 
        => Ok( await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Reservation>> Get(Guid id)
    {
        var reservation = await _service.GetAsync(id);
        if (reservation is null)
        {
            return NotFound();
        }
        return Ok(reservation);
    }

    [HttpPost("employee")]
    public async Task<ActionResult> Post(ReserveMachineForEmployee command)// #refactor name
    {
        //bo tworzac wg resta add nie podajesz godziny xD tuq

        var id = await _service.ReserveForEmployeeAsync(command with { ReservationId = Guid.NewGuid() });
        if (id is null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { id }, null);
        //   return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null); #refactor
        
    }
    [HttpPost("service")]
    public async Task<ActionResult> Post(ReserveMachineForService command) // #refactor name
    { 
         await _service.ReserveAllMachinesForServiceAsync(command); 
        return Ok();
        
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationHour command)
    {
        if (await _service.ChangeReservationHourAsync(command with { ReservationId = id }))
        {
            return NoContent();
        }
        return NotFound();
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (await _service.DeleteAsync(new DeleteReservation(id)))
        {
            return NoContent();
        }
        return NotFound();
    }
}
