using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductionScheduler.Infrastructure;

namespace ProductionScheduler.Api.Controllers
{
    [Route("")] // #22 - settings from json app settings
    public class HomeController : ControllerBase
    {
        private readonly string _name;
        public HomeController(IOptions<AppOptions> options) //IOptionsSnapshot - constant changes but memory hurting//extensions.options // 
        {
            _name = options.Value.Name; 
        }
        [HttpGet]
        public ActionResult<string> Get() => _name;
    }
}
