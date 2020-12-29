using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ApiWorld.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //to get the api-key from QueryString
            //context.HttpContext.Request.Query["api-key"]
            if (!context.HttpContext.Request.Headers.TryGetValue("api-key", out var apiKeyFromRequest))
            {
                context.Result = new UnauthorizedResult();
                // here the middleware will short circut and not forward request is made
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("api-key");
            if(apiKey != apiKeyFromRequest)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // the below code is used to call the next middlerware
            await next();
        }
    }
}
