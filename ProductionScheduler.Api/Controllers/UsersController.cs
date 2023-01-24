using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandHandler<SignUp> _signUpHandler;
        private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
        private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
        public UsersController(ICommandHandler<SignUp> signUpHandler, IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler, IQueryHandler<GetUser, UserDto> getUserHandler)
        {
            _signUpHandler = signUpHandler;
            _getUsersHandler = getUsersHandler;
            _getUserHandler = getUserHandler;
        }

        [HttpGet("{userId:guid}")] 
        public async Task<ActionResult<UserDto>> Get(Guid userId)
        {
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
            if (user is null)
            {
                return NotFound();
            } 
            return user;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Get()
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return NotFound();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });

            return user;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
      => Ok(await _getUsersHandler.HandleAsync(query));

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(SignUp command)
        {
            //nadpisanie commendy
            command = command with { UserId = Guid.NewGuid() };

            await _signUpHandler.HandleAsync(command);
            return CreatedAtAction(nameof(Get), new { command.UserId }, null); 
        }
        //[HttpPost("sign-in")]
        //[SwaggerOperation("Sign in the user and return the JSON Web Token")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<JwtDto>> Post(SignIn command)
        //{
        //    await _signInHandler.HandleAsync(command);
        //    var jwt = _tokenStorage.Get();
        //    return jwt;
        //}
    }

}
