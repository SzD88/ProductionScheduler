using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionScheduler.Application.Abstractions;
using ProductionScheduler.Application.Commands;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Queries;
using ProductionScheduler.Application.Security;

namespace ProductionScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase

    {
        private readonly ICommandHandler<SignUp> _signUpHandler;
        private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
        private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
        private readonly IAuthenticator _authenticator;
        private readonly ICommandHandler<SignIn> _signInHandler;
        private readonly ITokenStorage _tokenStorage;

        //  Unable to resolve service for type 'ProductionScheduler.Core.Repositories.IUserRepository'
        //   while attempting to activate 'ProductionScheduler.Application.Commands.Handlers.SignUpHandler'."
        // nie zarejestrowano kontenera DI a probowano go uzyc ... 
        //  services.AddScoped<IUserRepository, MSSqlUserRepository>(); // tego brakowalo #refactor zapamietać

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
        [HttpGet("jwt")]
        public async Task<ActionResult<JwtDto>> GetJWT()
        {
            var userId = Guid.NewGuid();
            var jwt = _authenticator.CreateToken(
                userId, "user"
                );

            return jwt;
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
        [Authorize] // [Authorize(AuthenticationSchemes = ...)] //  tu sie tez da wskazac ze jest bearer ale masz to gdzie indziej juz wpisane

        public async Task<ActionResult<UserDto>> GetCosTam()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.User.Identity?.Name))
            {
                return NotFound();
            }

            var userId = Guid.Parse(HttpContext.User.Identity?.Name);
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
            if (user is null)
                return NotFound();
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
        [HttpPost("sign-in")]
        // [SwaggerOperation("Sign in the user and return the JSON Web Token")]
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

