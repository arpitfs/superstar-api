using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorld.Controllers
{
    public class WorldController : Controller
    {
        [HttpGet("api/World")]
        public IActionResult Get()
        {
            return  Ok("Arpit");
        }
    }
}
