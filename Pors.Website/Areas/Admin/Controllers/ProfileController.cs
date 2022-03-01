using System;
using System.Linq;
using Loby.Extensions;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Profiles.Queries;
using Pors.Application.Profiles.Commands;

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
            var profile = new GetProfileQueryResponse();

            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    profile = await Mediator.Send(new GetProfileQuery());
                }

                return RedirectToAction(nameof(Index));
            }

            profile = await Mediator.Send(new GetProfileQuery());

            return View(profile);
        }
    }
}
