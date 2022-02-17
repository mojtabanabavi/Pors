using System;
using System.Linq;
using System.Threading.Tasks;
using Pors.Website.Extensions;
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
        public async Task<IActionResult> GetExams(DataTableRequest request3)
        {
            var dataTableRequest = BindDataTableRequest();

            var request = new GetExamsQuery
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

        #endregion;

        public async Task<IActionResult> Index(GetExamsQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
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

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);
                }

                return RedirectToAction(nameof(Index));
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

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteExamCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index));
        }
    }
}
