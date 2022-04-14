using System;
using Pors.Website.Constants;
using System.Security.Claims;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Pors.Application.Public.Identity.Queries;

namespace Pors.Website.Controllers
{
    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserQuery request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (result.IsSucceeded)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim("SessionId", request.SessionId),
                    };

                    var claimsIdentity = new ClaimsIdentity(userClaims, AuthenticationSchemes.Public);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(AuthenticationSchemes.Public, claimsPrincipal, authenticationProperties);

                    var cookieOption = new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        Expires = DateTimeOffset.Now.AddDays(15),
                    };

                    HttpContext.Response.Cookies.Append("AttempterId", request.SessionId, cookieOption);

                    return Ok();
                }
                else
                {
                    ModelState.AddErrors(result.Errors);

                    return BadRequest(ModelState.GetErrorMessages());
                }
            }

            return BadRequest(ModelState.GetErrorMessages());
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthenticationSchemes.Public);

            return RedirectToAction("index", "home");
        }
    }
}