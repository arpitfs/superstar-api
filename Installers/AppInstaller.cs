using ApiWorld.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiWorld.Installers
{
    public class AppInstaller : IInstaller
    {
        public void Install(IConfiguration configuration, IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Api World", Version = "v1" });
            });
        }
    }
}
