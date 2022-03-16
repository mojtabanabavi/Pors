using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Attempts.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت شرکت‌کنندگان")]
    public class AttemptController : BaseController
    {
        #region api

        [HttpPost]
        [DisplayName("دریافت لیست شرکت‌کنندگان")]
        public async Task<IActionResult> GetAttempts(int examId)
        {
            var query = DataTable.FetchRequest();

            var request = new GetAttemptsQuery(query, examId);

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
        [DisplayName("لیست شرکت‌کنندگان")]
        public IActionResult Index(int id)
        {
            return View(id);
        }
    }
}
