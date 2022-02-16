using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Exams.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExamCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (result.IsSucceeded)
                {
                    ViewBag.Message = result.Message;
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
