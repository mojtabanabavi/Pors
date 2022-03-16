using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Answers.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت پاسخ‌ها")]
    public class AnswerController : BaseController
    {
        [HttpPost]
        [DisplayName("دریافت لیست پاسخ‌ها")]
        public async Task<IActionResult> GetAnswers(int questionId)
        {
            var query = DataTable.FetchRequest();

            var request = new GetAnswersQuery(query, questionId);

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
        [DisplayName("لیست پاسخ‌ها")]
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        [DisplayName("جزئیات پاسخ")]
        public async Task<IActionResult> Details(GetAnswerQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }
    }
}
