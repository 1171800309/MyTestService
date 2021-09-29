using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TestService.Controllers
{
    public class HealthController : Common.Controller.BaseController
    {
        [HttpGet]
        public IActionResult GetAssemblyName()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
