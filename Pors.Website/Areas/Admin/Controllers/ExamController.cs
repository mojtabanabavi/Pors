using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Exams.Queries;
using Pors.Application.Exams.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        #region api

        [HttpPost]
        public async Task<IActionResult> GetExams()
        {
            var query = DataTable.FetchRequest();

            var request = new GetExamsQuery(query);

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
                var examId = await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetExamQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateExamCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var exam = await Mediator.Send(new GetExamQuery(request.Id));

            return View(exam);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteExamCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
