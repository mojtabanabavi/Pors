using System;
using System.Linq;
using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Common.Models;
using Pors.Application.Options.Queries;
using Pors.Application.Options.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class OptionController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> GetOptions()
        {
            var dataTableRequest = BindDataTableRequest();

            var questionIdTemp = Request.Form["id"].FirstOrDefault();
            var questionId = questionIdTemp != null ? Convert.ToInt32(questionIdTemp) : 0;

            var request = new GetOptionsQuery
            {
                Id = questionId,
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

        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = new CreateOptionsCommand
            {
                Id = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOptionsCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    return View(request);
                }

                return RedirectToAction(nameof(Index), new { id = request.Id });
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetOptionQuery request)
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
        public async Task<IActionResult> Update(UpdateOptionCommand request)
        {
            var model = new GetOptionQueryResponse();

            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    model = new GetOptionQueryResponse
                    {
                        Id = request.Id,
                        Title = request.Title,
                    };

                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            model = new GetOptionQueryResponse
            {
                Id = request.Id,
                Title = request.Title,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteOptionCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    //return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index));
        }
    }
}