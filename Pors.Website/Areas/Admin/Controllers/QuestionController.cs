using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Questions.Queries;
using Pors.Application.Management.Questions.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت سوالات")]
    public class QuestionController : BaseController
    {
        [HttpPost]
        [DisplayName("دریافت لیست سوالات")]
        public async Task<IActionResult> GetQuestions(int examId)
        {
            var query = DataTable.FetchRequest();

            var request = new GetQuestionsQuery(query, examId);

            var result = await Mediator.Send(request);

            var jsonData = new DataTableResponse
            {
                Draw = query.Draw,
                Data = result.Items,
                RecordsTotal = result.TotalItems,
                RecordsFiltered = result.TotalItems,
            };

            return Json(jsonData);
        }

        [HttpGet]
        [DisplayName("لیست سوالات")]
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        [DisplayName("ایجاد سوال")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DisplayName("ایجاد سوال")]
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
        [DisplayName("ویرایش سوال")]
        public async Task<IActionResult> Update(GetQuestionQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش سوال")]
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
        [DisplayName("حذف سوال")]
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
