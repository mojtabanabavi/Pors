using Pors.Website.Constants;
using System.Security.Claims;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Users.Queries;
using Microsoft.AspNetCore.Authentication;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class IdentityController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserQuery query)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(query);

                if (result.IsSucceeded)
                {
                    var response = result.Data;

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,response.DisplayName),
                        new Claim("ProfilePicture",response.User.ProfilePicture),
                        new Claim(ClaimTypes.NameIdentifier,response.User.Id.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(userClaims, AuthenticationSchemes.Admin);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
                }
                else
                {
                    ModelState.AddErrors(result.Errors);
                }

                return RedirectToAction();
            }

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthenticationSchemes.Admin);

            return RedirectToAction(nameof(Login));
        }
    }
}