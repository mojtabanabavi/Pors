using Pors.Website.Constants;
using System.Security.Claims;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Identity.Queries;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Identity.Commands;
using Microsoft.AspNetCore.Authentication;

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

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserQuery request)
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

                    var claimsIdentity = new ClaimsIdentity(userClaims, AuthenticationSchemes.Admin);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(AuthenticationSchemes.Admin, claimsPrincipal, authenticationProperties);

                    if (!string.IsNullOrEmpty(request.ReturnUrl))
                    {
                        return Redirect(request.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
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
            await HttpContext.SignOutAsync(AuthenticationSchemes.Admin);

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
                var result = await Mediator.Send(request);

                if (result.IsSucceeded)
                {
                    ViewBag.SendTokenMessage = result.Message;
                }
                else
                {
                    ModelState.AddErrors(result.Errors);
                }
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
                var result = await Mediator.Send(request);

                if (result.IsSucceeded)
                {
                    ViewBag.SendTokenMessage = result.Message;
                }
                else
                {
                    ModelState.AddErrors(result.Errors);
                }
            }

            return View(request);
        }
    }
}