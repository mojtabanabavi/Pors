using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Pors.Website.Middlewares
{
    public class CookieInitializerMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly string CookieKey = "AttempterId";

        public CookieInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey(CookieKey))
            {
                var userId = Guid.NewGuid().ToString();
                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.Now.AddDays(15),
                };

                context.Response.Cookies.Append(CookieKey, userId, cookieOption);
            }

            await _next(context);
        }
    }

    public static class cookieInitializerMiddlewareExtension
    {
        public static void UsecookieInitializer(this IApplicationBuilder app)
        {
            app.UseMiddleware<CookieInitializerMiddleware>();
        }
    }
}