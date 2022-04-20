using System;
using Loby.Extensions;
using Pors.Website.Constants;
using System.Security.Claims;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Pors.Application.Management.Identity.Queries;
using Pors.Application.Management.Identity.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Identity", new { ReturnUrl = returnUrl });

            var authenticationProperties = new AuthenticationProperties { RedirectUri = redirectUrl };

            authenticationProperties.Items["LoginProvider"] = provider;

            return new ChallengeResult(provider, authenticationProperties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var googleUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(new ExternalLoginQuery(userEmail));

                if (result.IsSucceeded)
                {
                    var response = result.Data;

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,response.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier,response.User.Id.ToString()),
                    };

                    if (response.User.ProfilePicture != null)
                    {
                        userClaims.Add(new Claim("ProfilePicture", response.User.ProfilePicture));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, AuthenticationSchemes.Management);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(AuthenticationSchemes.Management, claimsPrincipal, authenticationProperties);

                    if (returnUrl.HasValue() && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "dashboard");
                    }
                }
                else
                {
                    ModelState.AddErrors(result.Errors);

                    await HttpContext.SignOutAsync(AuthenticationSchemes.Management);
                }
            }

            return View(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserQuery request, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (result.IsSucceeded)
                {
                    var response = result.Data;

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,response.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier,response.User.Id.ToString()),
                    };

                    if (response.User.ProfilePicture != null)
                    {
                        userClaims.Add(new Claim("ProfilePicture", response.User.ProfilePicture));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, AuthenticationSchemes.Management);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = request.RememberMe,
                    };

                    await HttpContext.SignInAsync(AuthenticationSchemes.Management, claimsPrincipal, authenticationProperties);

                    if (returnUrl.HasValue() && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "dashboard");
                    }
                }
                else
                {
                    ModelState.AddErrors(result.Errors);
                }
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthenticationSchemes.Management);

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(SendForgetPasswordTokenCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(ResetPassword));
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordCommand(id));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Login));
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("_errorPage", 403);
        }
    }
}