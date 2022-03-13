using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Attempts.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
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
        public IActionResult Index(int id)
        {
            return View(id);
        }
    }
}
