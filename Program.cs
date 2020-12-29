using ApiWorld.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ApiWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Migration at service startUp

        //public static async Task Main1(string[] args)
        //{
        //    var host = CreateHostBuilder(args).Build();

        //    using (var service = host.Services.CreateScope())
        //    {
        //        var dbContext = service.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //        await dbContext.Database.MigrateAsync();

        //        var roleManager = service.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        if(!await roleManager.RoleExistsAsync("Admin"))
        //        {
        //            var adminRole = new IdentityRole("Admin");
        //            await roleManager.CreateAsync(adminRole);
        //        }
        //    }

        //    await host.RunAsync();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}