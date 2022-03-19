using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
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
        [DisplayName("دریافت لیست آزمون‌ها")]
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
        public async Task<IActionResult> GetQuestionAnswersChartData(GetQuestionAnswersChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        public async Task<IActionResult> GetQuestionAnswersAccuracyChartData(GetQuestionAnswersAccuracyChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }


        #endregion;

        [HttpGet]
        [DisplayName("لیست آزمون‌ها")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [DisplayName("ایجاد آزمون")]
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
        public async Task<IActionResult> Report(GetExamReportQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }
    }
}
