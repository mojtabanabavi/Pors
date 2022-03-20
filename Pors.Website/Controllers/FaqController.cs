using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Public.Faqs.Queries;

namespace Pors.Website.Controllers
{
    public class FaqController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetFaqsQuery());

            return View(result);
        }
    }
}
