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

            var examIdTemp = Request.Form["id"].FirstOrDefault();
            var examId = examIdTemp != null ? Convert.ToInt32(examIdTemp) : 0;

            var request = new GetQuestionsQuery
            {
                Id = examId,
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
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
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
                var questionId = await Mediator.Send(request);

                return RedirectToAction(nameof(Index), new { id = request.ExamId });
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetQuestionQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateQuestionCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index), new { id = request.Id });
            }

            var question = await Mediator.Send(new GetQuestionQuery(request.Id));

            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteQuestionCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index), new { id = request.Id });
        }
    }
}
