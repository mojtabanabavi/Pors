using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pors.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
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

            if (env.IsDevelopment())
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<SqlDbContext>();

                        await context.Database.MigrateAsync();

                        await SqlDbContextSeed.SeedDefaultUserAsync(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    }
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
