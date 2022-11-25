using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductionScheduler.Infrastructure;

namespace ProductionScheduler.Api.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly string _name;
        public HomeController(IOptions<AppOptions> options) //IOptionsSnapshot //extensions.options
        {
            _name = options.Value.Name; 
        }
        [HttpGet]
        public ActionResult<string> Get() => _name;
    }
}
