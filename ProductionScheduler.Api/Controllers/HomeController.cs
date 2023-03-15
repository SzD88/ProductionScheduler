using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductionScheduler.Infrastructure;
using WebApi.Controllers;

namespace ProductionScheduler.Api.Controllers
{
    [Route("")]
    public class HomeController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        public ILogger<HomeController> _logger { get; }

        private readonly AppOptions _appOptions;
        public HomeController(IOptions<AppOptions> options,  IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            _appOptions = options.Value;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<string> Get()
        {
            _memoryCache.Remove("machines");
            _logger.LogInformation("Machines cleared from cache");
            return Ok(_appOptions.Name);
        } 
    }
}
