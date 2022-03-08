﻿using System;
using System.Linq;
using System.Threading.Tasks;
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
            var query = DataTable.FetchRequest();

            var questionId = Convert.ToInt32(Request.Form["id"].FirstOrDefault());

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
        public IActionResult Index(int id)
        {
            return View(id);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            return View(new CreateOptionsCommand(id));
        }

        [HttpPost]
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
        public async Task<IActionResult> Update(GetOptionQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
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