using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ApiWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Migration at service startUp

        //public static async Task Main(string[] args)
        //{
        //    var host = CreateHostBuilder(args).Build();

        //    using(var service = host.Services.CreateScope())
        //    {
        //        var dbContext = service.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //        await dbContext.Database.MigrateAsync();
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