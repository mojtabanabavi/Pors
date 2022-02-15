using System;
using System.Linq;
using Pors.Application;
using Pors.Infrastructure;
using Pors.Website.Services;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
            services.AddFluentValidation();
            services.AddHttpContextAccessor();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddAuthentication(AuthenticationSchemes.Default)
                .AddCookie(AuthenticationSchemes.Default, options =>
                {
                    options.LoginPath = "/identity/login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.LogoutPath = "/identity/logout";
                })
                .AddCookie(AuthenticationSchemes.Admin, options =>
                {
                    options.LoginPath = "/admin/identity/login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.LogoutPath = "/admin/identity/logout";
                });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
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
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
