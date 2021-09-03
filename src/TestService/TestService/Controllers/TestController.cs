using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestService.Controllers
{
 
    public class TestController :Common.Controller.BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetTest()
        {
            return await Task.Run(() =>
            {
                return Ok("This's my test api!");
            });
        }
    }
}
