using ApiWorld.Domain;
using System.Threading.Tasks;

namespace ApiWorld.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationRequest> RegisterAsync(string email, string password);
        Task<AuthenticationRequest> LoginAsync(string email, string password);
    }
}