using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GateWayService.Controllers
{

    public class DefaultController : Common.Controller.BaseController
    { 
        /// <summary>
        /// 健康检查
         /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAssemblyName()
        {
            string str = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            return Ok(str + Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
