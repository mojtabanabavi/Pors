using System;
using System.Linq;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Exams.Queries;
using Pors.Application.Questions.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pors.Application.Questions.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> GetQuestions()
        {
            var dataTableRequest = BindDataTableRequest();

            var request = new GetQuestionsQuery
            {
                Page = dataTableRequest.Page,
                Search = dataTableRequest.Search,
                PageSize = dataTableRequest.PageSize,
                SortColumn = dataTableRequest.SortColumn,
                SortColumnDirection = dataTableRequest.SortColumnDirection
            };

            var result = await Mediator.Send(request);

            var jsonData = new DataTableResponse
            {
                Data = result.Items,
                Draw = dataTableRequest.Draw,
                RecordsTotal = result.TotalItems,
                RecordsFiltered = result.TotalItems,
            };

            return Json(jsonData);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var result = await Mediator.Send(new GetExamsSelectListQuery());

            ViewBag.ExamsSelectList = new SelectList(result.Items, "Text", "Value");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetQuestionQuery request)
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
        public async Task<IActionResult> Update(UpdateQuestionCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
