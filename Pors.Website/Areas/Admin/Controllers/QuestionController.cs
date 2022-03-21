using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Questions.Queries;
using Pors.Application.Management.Questions.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت سوالات")]
    public class QuestionController : BaseController
    {
        #region api

        [HttpPost]
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

        #endregion;

        [HttpGet]
        [DisplayName("لیست سوالات")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        [DisplayName("ایجاد سوال")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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
