using System.Collections.Generic;

namespace ApiWorld.Domain
{
    public class AuthenticationRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
