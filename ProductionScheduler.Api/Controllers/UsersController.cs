using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using ProductionScheduler.Application.Security;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        private readonly ICommandHandler<SignUp> _signUpHandler;
        private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
        private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
        private readonly IAuthenticator _authenticator;
        private readonly ICommandHandler<SignIn> _signInHandler;
        private readonly ITokenStorage _tokenStorage;

        public UsersController(ICommandHandler<SignUp> signUpHandler, IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
            IQueryHandler<GetUser, UserDto> getUserHandler, IAuthenticator authenticator, ICommandHandler<SignIn> signInHandler, ITokenStorage tokenStorage)
        {
            _signUpHandler = signUpHandler;
            _getUsersHandler = getUsersHandler;
            _getUserHandler = getUserHandler;
            _authenticator = authenticator;
            _signInHandler = signInHandler;
            _tokenStorage = tokenStorage;
        }

        [HttpGet("{userId:guid}")]
        [SwaggerOperation("Retrieves user by id")]
        [Authorize(Policy = "is-admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(Guid userId)
        {
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
            return OkOrNotFound(user);
        }

        [HttpGet]
        [SwaggerOperation("Retrieves all users")]
        [Authorize(Policy = "is-admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] GetUsers query)
         => Ok(await _getUsersHandler.HandleAsync(query));

        [HttpGet("me")]
        [SwaggerOperation("Retrieves the currently logged in user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> GetSelf()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.User.Identity?.Name))
            {
                return NotFound();
            }
            var role =  HttpContext.User.IsInRole("user");
            var userId = Guid.Parse(HttpContext.User.Identity?.Name);
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
            
            return OkOrNotFound(user);
        }

        [HttpPost]
        [SwaggerOperation("Create a user account")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(SignUp command)
        { 
            command = command with { UserId = Guid.NewGuid() };

            await _signUpHandler.HandleAsync(command);
            return CreatedAtAction(nameof(GetUser), new { command.UserId }, null);
        }

        [HttpPost("sign-in")]
        [SwaggerOperation("Sign in the user and return the JSON Web Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JwtDto>> Post(SignIn command)
        {
            await _signInHandler.HandleAsync(command);
            var jwt = _tokenStorage.Get();
            return Ok(jwt);
        }
    } 
}

