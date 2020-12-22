using ApiWorld.Contracts.V1;
using ApiWorld.Contracts.V1.Request;
using ApiWorld.Contracts.V1.Response;
using ApiWorld.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWorld.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if(!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse { Errors = authResponse.ErrorMessages });
            }

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }
    }
}
