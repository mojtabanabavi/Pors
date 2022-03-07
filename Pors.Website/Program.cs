using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pors.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Website
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var env = host.Services.GetRequiredService<IWebHostEnvironment>();

            if (env.IsProduction())
            {
                using (var scope = host.Services.CreateScope())
                {
                    var databaseSeed = scope.ServiceProvider.GetRequiredService<ISqlDbContextSeed>();

                    await databaseSeed.SeedDataAsync();
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return host;
        }
    }
}
