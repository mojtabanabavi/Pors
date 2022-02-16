using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Exams.Queries;
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

        [HttpGet]
        public async Task<IActionResult> Update(GetExamQuery request)
        {
            var result = await Mediator.Send(request);

            if (result.IsSucceeded)
            {
                return View(result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateExamCommand request)
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

            return View();
        }
    }
}
