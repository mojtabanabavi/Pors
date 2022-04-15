using System;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pors.Website.Filters
{
    public class SetParticipantIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Cookies
                .ContainsKey(ParticipantCookieKeys.ParticipantId))
            {
                var participantId = Guid.NewGuid().ToString();

                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.Now.AddDays(15),
                };

                context.HttpContext.Response.Cookies
                    .Append(ParticipantCookieKeys.ParticipantId, participantId, cookieOption);
            }

            base.OnActionExecuting(context);
        }
    }
}
