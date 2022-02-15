using System;
using System.Collections.Generic;
using Pors.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Pors.Infrastructure.Persistence;
using Pors.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                options => options.MigrationsAssembly(typeof(SqlDbContext).Assembly.FullName)));

            services.AddScoped<ISqlDbContext, SqlDbContext>();
            services.AddScoped<ITokenBuilderService, TokenBuilderService>();
            services.AddScoped<INotificationService, EmailNotificationService>();

            services.Configure<EmailNotificationService.Settings>(x=> configuration.GetSection("Notifications:Email"));

            return services;
        }
    }
}
