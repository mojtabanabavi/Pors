using System;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Management.Profiles.Queries;
using Pors.Application.Management.Profiles.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetProfileQuery());

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateProfileCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var profile = await Mediator.Send(new GetProfileQuery());

            return View(profile);
        }
    }
}
