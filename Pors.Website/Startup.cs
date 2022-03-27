using System;
using Loby.Tools;
using Pors.Application;
using Pors.Infrastructure;
using System.Globalization;
using Pors.Website.Filters;
using Pors.Website.Services;
using Pors.Website.Policies;
using Pors.Website.Constants;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pors.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Pors.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new ExceptionHandlerAttribute());
            }).AddRazorRuntimeCompilation();

            services.AddFluentValidation(options =>
            {
                options.ValidatorOptions.LanguageManager.Culture = new CultureInfo("fa");
            });

            services.AddAuthentication()
                .AddCookie(AuthenticationSchemes.Management, options =>
                {
                    options.LoginPath = "/admin/identity/login";
                    options.LogoutPath = "/admin/identity/logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.AccessDeniedPath = "/admin/identity/accessDenied";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.DynamicPermission, policy =>
                {
                    policy.Requirements.Add(new DynamicPermissionRequirement());
                });
            });

            services.AddScoped<IDataTableService, DataTableService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthorizationHandler, DynamicPermissionHandler>();
            services.AddScoped<IControllerDiscoveryService, ControllerDiscoveryService>();
            services.Configure<MailerSettings>(Configuration.GetSection("Notifications:Email"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseWhen(context => context.Request.Path.StartsWithSegments("/admin"), app =>
                {
                    app.UseStatusCodePagesWithReExecute("/admin/error", "?statusCode={0}");
                });

                app.UseWhen(context => !context.Request.Path.StartsWithSegments("/admin"), app =>
                {
                    app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");
                });
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
                  pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}"
                );
            });
        }
    }
}
