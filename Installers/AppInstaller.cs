using ApiWorld.Installer;
using ApiWorld.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace ApiWorld.Installers
{
    public class AppInstaller : IInstaller
    {
        public void Install(IConfiguration configuration, IServiceCollection services)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddControllersWithViews();
            services.AddRazorPages();

            // Setup JWT token authentication for an api
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false

                };
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo { Title = configuration["SwaggerOptions:Description"],
                        Version = "v1" });

                // To configure swagger with the authorization
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0]}
                };

                x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT token authentication",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
            });
        }
    }
}
