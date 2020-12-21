using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiWorld.Installer
{
    public interface IInstaller
    {
        void Install(IConfiguration configuration, IServiceCollection services);
    }
}
