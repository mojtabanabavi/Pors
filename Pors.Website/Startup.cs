using System;
using Pors.Application;
using Pors.Infrastructure;
using System.Globalization;
using Pors.Website.Services;
using Pors.Website.Constants;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pors.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Pors.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Website
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddSession();
            services.AddHttpContextAccessor();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddFluentValidation(options =>
            {
                options.ValidatorOptions.LanguageManager.Culture = new CultureInfo("fa");
            });

            services.AddAuthentication()
                .AddCookie(AuthenticationSchemes.Public, options =>
                {
                    options.LoginPath = "/identity/login";
                    options.LogoutPath = "/identity/logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                })
                .AddCookie(AuthenticationSchemes.Management, options =>
                {
                    options.LoginPath = "/admin/identity/login";
                    options.LogoutPath = "/admin/identity/logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });

            services.AddScoped<IDataTableService, DataTableService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IControllerDiscoveryService, ControllerDiscoveryService>();
            services.Configure<EmailNotificationService.Settings>(Configuration.GetSection("Notifications:Email"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=home}/{action=index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}"
                );
            });
        }
    }
}
