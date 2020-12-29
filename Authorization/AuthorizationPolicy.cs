using Microsoft.AspNetCore.Authorization;

namespace ApiWorld.Authorization
{
    public class AuthorizationPolicy : IAuthorizationRequirement
    {
        public string DomainName { get; }

        public AuthorizationPolicy(string domain)
        {
            DomainName = domain;
        }
    }
}
