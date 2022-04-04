using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Options.Queries;
using Pors.Application.Management.Options.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت گزینه‌ها")]
    public class OptionController : BaseController
    {
        #region api

        [HttpPost]
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

        #endregion;

        [HttpGet]
        [DisplayName("لیست گزینه‌ها")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        [DisplayName("ایجاد گزینه")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Create(int id)
        {
            return View(new CreateOptionsCommand(id));
        }

        [HttpPost]
        [DisplayName("ایجاد گزینه")]
        public async Task<IActionResult> Create([FromForm] CreateOptionsCommand request)
        {
            if (ModelState.IsValid)
            {
                if(Request.Form.Files != null)
                {
                    // Binding options file
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        request.Items[i].Image = Request.Form.Files[$"items[{i}][image]"];
                    }
                }

                var optionIds = await Mediator.Send(request);

                return RedirectToAction(nameof(Index), new { id = request.Id });
            }

            return View(request);
        }

        [HttpGet]
        [DisplayName("ویرایش گزینه")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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