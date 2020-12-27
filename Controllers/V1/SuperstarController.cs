using ApiWorld.Domain;
using Microsoft.AspNetCore.Mvc;
using ApiWorld.Contracts.V1;
using System;
using ApiWorld.Contracts.V1.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiWorld.Repository;

namespace ApiWorld.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SuperstarController : Controller
    {
        private readonly ISuperStarRepository _superStarRepository;

        public SuperstarController(ISuperStarRepository superStarRepository)
        {
            _superStarRepository = superStarRepository;
        }

        [HttpGet(ApiRoutes.SuperStar.GetAll)]
        public IActionResult Get()
        {
            var superStars = _superStarRepository.GetAll();
            return Ok(superStars);
        }

        [HttpPost(ApiRoutes.SuperStar.Create)]
        public IActionResult Create([FromBody] CreateSuperstarRequest superStarRequest)
        {
            var superStar = new SuperStar
            {
                Smack = superStarRequest.Smack,
                Height = superStarRequest.Height,
                Name = superStarRequest.Name,
                SuperstarId = Guid.NewGuid().ToString(),
                Weight = superStarRequest.Weight
            };

            _superStarRepository.Save(superStar);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = baseUrl + "/" + ApiRoutes.SuperStar.Get.Replace("{superstarId}", superStar.SuperstarId);

            var createdSuperstar = new CreateSuperstarResponse
            {
                Smack = superStar.Smack,
                Height = superStar.Height,
                Name = superStar.Name,
                SuperstarId = superStar.SuperstarId,
                Weight = superStar.Weight
            };

            return Created(location, createdSuperstar);
        }
    }
}
