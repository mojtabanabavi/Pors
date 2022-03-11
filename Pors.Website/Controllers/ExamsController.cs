﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Public.Exams.Queries;
using Pors.Application.Public.ExamAttempts.Commands;

namespace Pors.Website.Controllers
{
    public class answer
    {
        public List<items> answers { get; set; }

        public class items
        {
            public int QuestionId { get; set; }
            public int AnswerId { get; set; }
        }
    }

    public class ExamsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Attempt(int id)
        {
            var attemptId = await Mediator.Send(new CreateExamAttemptCommand(id));

            return RedirectToAction(nameof(Start), new { Id = id , attempt = attemptId});
        }

        [HttpGet("start/{id}/{attempt}")]
        public async Task<IActionResult> Start(int id, string attempt)
        {
            var result = await Mediator.Send(new GetExamQuery(id));

            return View(result);
        }

        [HttpPost]
        public IActionResult Answer(answer a)
        {


            return View();
        }
    }
}