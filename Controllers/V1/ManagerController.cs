using ApiWorld.Contracts.V1;
using ApiWorld.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        // Custom claim policy for the endpoint present in the token
        public IActionResult GetAll()
        {
            return Ok(_managerRepository.GetAll());
        }

        [HttpDelete(ApiRoutes.Manager.Delete)]
        [Authorize(Policy = "AuthorizationPolicy")]
        // Custome authorization policy depending on the policy requirement the endpoint is accessed
        public async Task<IActionResult> Delete(string managerId)
        {
            return Ok(await _managerRepository.DeleteAsync(managerId));
        }
    }
}
