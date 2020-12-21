using System.Collections.Generic;
using ApiWorld.Domain;
using Microsoft.AspNetCore.Mvc;
using ApiWorld.Contracts.V1;
using System;
using ApiWorld.Contracts.V1.Response;

namespace ApiWorld.Controllers.V1
{
    public class SuperstarController : Controller
    {
        public readonly List<SuperStar> _superStars;

        public SuperstarController()
        {
            _superStars = new List<SuperStar>
            {
                new SuperStar { SuperstarId = Guid.NewGuid().ToString(), Name = "Undertaker", Height = "6'10", Weight = "320 pounds", Smack = "Chockslam"},
                new SuperStar { SuperstarId = Guid.NewGuid().ToString(), Name = "Rock", Height = "6'5", Weight = "265 pounds", Smack = "RockBottom"}
            };
        }

        [HttpGet(ApiRoutes.SuperStar.GetAll)]
        public IActionResult Get()
        {
            return Ok(_superStars);
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

            _superStars.Add(superStar);
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
