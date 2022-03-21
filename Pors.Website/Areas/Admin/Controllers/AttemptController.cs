using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Attempts.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت شرکت‌کنندگان")]
    public class AttemptController : BaseController
    {
        #region api

        [HttpPost]
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
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index(int id)
        {
            return View(id);
        }
    }
}
