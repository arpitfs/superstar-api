using System.Collections.Generic;
using ApiWorld.Domain;
using Microsoft.AspNetCore.Mvc;
using ApiWorld.Contracts.V1;

namespace ApiWorld.Controllers.V1
{
    public class SuperstarController : Controller
    {
        public readonly List<SuperStar> _superStars;

        public SuperstarController()
        {
            _superStars = new List<SuperStar>
            {
                new SuperStar { Name = "Undertaker", Height = "6'10", Weight = "320 pounds", Smack = "Chockslam"},
                new SuperStar { Name = "Rock", Height = "6'5", Weight = "265 pounds", Smack = "RockBottom"}
            };
        }
        [HttpGet(ApiRoutes.SuperStar.GetAll)]
        public IActionResult Get()
        {
            return Ok(_superStars);
        }
    }
}
