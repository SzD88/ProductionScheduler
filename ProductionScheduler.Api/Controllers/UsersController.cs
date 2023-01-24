using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandHandler<SignUp> _signUpHandler;
        public UsersController(ICommandHandler<SignUp> signUpHandler)
        {
            _signUpHandler = signUpHandler;
        }
        [HttpPost]
        public async Task<ActionResult> Post(SignUp command)
        {
            //nadpisanie commendy
            command = command with { UserId = Guid.NewGuid() };

            await _signUpHandler.HandleAsync(command);
            return NoContent();

            //  return CreatedAtAction(); //#refactor
        }
        //refactor - wystawic pozostale akcje 

    }

}
