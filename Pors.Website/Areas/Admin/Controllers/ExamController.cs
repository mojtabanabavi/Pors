using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Exams.Queries;
using Pors.Application.Management.Exams.Commands;
using Pors.Application.Management.Reports.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت آزمون‌ها")]
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

        [HttpPost]
        public async Task<IActionResult> GetQuestionAnswersChartData(GetAnswersChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetQuestionAnswersAccuracyChartData(GetAnswersAccuracyChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetGetExamVisitsChartData(GetExamVisitsChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetGetExamsVisitsChartData()
        {
            var result = await Mediator.Send(new GetExamsVisitsChartDataQuery());

            return Json(result);
        }

        #endregion;

        [HttpGet]
        [DisplayName("لیست آزمون‌ها")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [DisplayName("ایجاد آزمون")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DisplayName("ایجاد آزمون")]
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
        [DisplayName("ویرایش آزمون")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Update(GetExamQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش آزمون")]
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
        [DisplayName("حذف آزمون")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Delete(DeleteExamCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [DisplayName("مشاهده‌ی گزارش")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Report(GetExamStatusReportQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }
    }
}
