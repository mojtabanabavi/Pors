﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Answers.Queries;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت پاسخ‌ها")]
    public class AnswerController : BaseController
    {
        #region api

        [HttpPost]
        public async Task<IActionResult> GetAnswers(int questionId,string participantId)
        {
            var query = DataTable.FetchRequest();

            var request = new GetAnswersQuery(query, questionId, participantId);

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
        [DisplayName("لیست پاسخ‌ها")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [DisplayName("جزئیات پاسخ")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Details(GetAnswerQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }
    }
}
