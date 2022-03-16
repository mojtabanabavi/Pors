using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Faqs.Queries;
using Pors.Application.Management.Faqs.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت سوالات متداول")]
    public class FaqController : BaseController
    {
        #region api

        [HttpPost]
        [DisplayName("دریافت لیست سوالات متداول")]
        public async Task<IActionResult> GetFaqs()
        {
            var query = DataTable.FetchRequest();

            var request = new GetFaqsQuery(query);

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
        [DisplayName("لیست سوالات متداول")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [DisplayName("ایجاد سوال متداول")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DisplayName("ایجاد سوال متداول")]
        public async Task<IActionResult> Create(CreateFaqCommand request)
        {
            if (ModelState.IsValid)
            {
                var faqId = await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        [DisplayName("ویرایش سوال متداول")]
        public async Task<IActionResult> Update(GetFaqQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش سوال متداول")]
        public async Task<IActionResult> Update(UpdateFaqCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var faq = await Mediator.Send(new GetFaqQuery(request.Id));

            return View(faq);
        }

        [HttpGet]
        [DisplayName("حذف سوال متداول")]
        public async Task<IActionResult> Delete(DeleteFaqCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
