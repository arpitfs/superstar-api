using ApiWorld.Contracts.V1;
using ApiWorld.Contracts.V1.Request;
using ApiWorld.Contracts.V1.Response;
using ApiWorld.Domain;
using ApiWorld.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost(ApiRoutes.Manager.Create)]
        public IActionResult Create([FromBody] CreateManagerRequest managerRequest)
        {
            var manager = new Manager
            {
                ManagerId = Guid.NewGuid().ToString(),
                Event = managerRequest.Event,
                IsCurrentManager = managerRequest.IsManager,
                Name = managerRequest.Name
            };

            _managerRepository.Save(manager);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = baseUrl + "/" + ApiRoutes.SuperStar.Get.Replace("{managerId}", manager.ManagerId);

            var createdManager = new CreateManagerResponse
            {
                Event = managerRequest.Event,
                IsManager = managerRequest.IsManager,
                Name = managerRequest.Name
            };

            return Created(location, createdManager);
        }
    }
}
