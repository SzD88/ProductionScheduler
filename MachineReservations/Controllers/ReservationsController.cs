
using Microsoft.AspNetCore.Mvc;
using MachineReservations.Api.Commands;
using MachineReservations.Api.Controllers.Models;
using MachineReservations.Api.Services;

namespace MachineReservations.Api.Controllers;

[ApiController] // to odpowiada za to ze nie trzeba uzywac ciagle from body - oznaczasz ze to jest tyylko api
[Route(template: "reservations")]
public class ReservationsController : ControllerBase
{

    private readonly Clock _clock;

    private readonly ReservationsService _service;

    public ReservationsController(ReservationsService service , Clock clock)
    {
        _service = service;
        _clock = clock;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_service.GetAllWeekly());
    [HttpGet("{id:guid}")]
    public ActionResult<Reservation> Get(Guid id) 
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }
        return Ok(reservation); 
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        //bo tworzac wg resta add nie podajesz godziny xD tuq

        var id = _service.Create(command with {ReservationId = Guid.NewGuid()});
        //na tym etapie przekazuje godzine
        if (id is null)
        {
            return BadRequest();
        } 
        return CreatedAtAction(nameof(Get), new { id }, null);
       // bylo return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null);
        //
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationHour command)
    {
        if (_service.Update( command with {ReservationId = id }))
        {
            return NoContent();
        }
        return NotFound(); 
    }
    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        if (_service.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }
        return NotFound();
    }
}
