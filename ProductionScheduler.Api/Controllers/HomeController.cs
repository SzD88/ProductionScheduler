using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductionScheduler.Infrastructure;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers
{
    [Route("")]
    public class HomeController : BaseController
    {
        private readonly AppOptions _appOptions;
        public HomeController(IOptions<AppOptions> options)
        {
            _appOptions = options.Value;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(_appOptions.Name);
        } 
    }
}
