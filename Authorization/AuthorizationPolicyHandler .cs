using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiWorld.Authorization
{
    public class AuthorizationPolicyHandler : AuthorizationHandler<AuthorizationPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationPolicy requirement)
        {
            //ClaimTypes come from the payload of a token
            var userEmailAddress = context.User?.FindFirst(ClaimTypes.Email).Value;
            if (userEmailAddress.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
         
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
