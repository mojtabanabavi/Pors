using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Options.Queries;
using Pors.Application.Management.Options.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت گزینه‌ها")]
    public class OptionController : BaseController
    {
        [HttpPost]
        [DisplayName("دریافت لیست گزینه‌ها")]
        public async Task<IActionResult> GetOptions(int questionId)
        {
            var query = DataTable.FetchRequest();

            var request = new GetOptionsQuery(query, questionId);

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
        [DisplayName("لیست گزینه‌ها")]
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        [DisplayName("ایجاد گزینه")]
        public IActionResult Create(int id)
        {
            return View(new CreateOptionsCommand(id));
        }

        [HttpPost]
        [DisplayName("ایجاد گزینه")]
        public async Task<IActionResult> Create(CreateOptionsCommand request)
        {
            if (ModelState.IsValid)
            {
                var optionIds = await Mediator.Send(request);

                return RedirectToAction(nameof(Index), new { id = request.Id });
            }

            return View(request);
        }

        [HttpGet]
        [DisplayName("ویرایش گزینه")]
        public async Task<IActionResult> Update(GetOptionQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش گزینه")]
        public async Task<IActionResult> Update(UpdateOptionCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var option = await Mediator.Send(new GetOptionQuery(request.Id));

            return View(option);
        }

        [HttpGet]
        [DisplayName("حذف گزینه")]
        public async Task<IActionResult> Delete(DeleteOptionCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}