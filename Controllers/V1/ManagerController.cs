using ApiWorld.Contracts.V1;
using ApiWorld.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorld.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ManagerController : Controller
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerController(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        [HttpGet(ApiRoutes.Manager.GetAll)]
        [Authorize(Policy = "Manager")]
        public IActionResult GetAll()
        {
            return Ok(_managerRepository.GetAll());
        }
    }
}
