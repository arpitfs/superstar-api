using ApiWorld.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorld.Controllers.V1
{
    [ApiKeyAuthAttribute]
    public class ApiKeyController : ControllerBase
    {
        [HttpGet("api/secret")]
        public IActionResult Get()
        {
            return Ok("Api key authentication");
        }
    }
}
