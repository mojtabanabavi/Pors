using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Public.Exams.Queries;
using Pors.Application.Public.ExamAttempts.Commands;
using Pors.Application.Public.AttemptAnswers.Commands;

namespace Pors.Website.Controllers
{
    public class ExamsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Attempt(int id)
        {
            var attemptId = await Mediator.Send(new CreateExamAttemptCommand(id));

            return RedirectToAction(nameof(Start), new { ExamId = id, AttemptId = attemptId });
        }

        [HttpGet("start/{examId}/{attemptId}")]
        public async Task<IActionResult> Start(int examId, string attemptId)
        {
            var result = await Mediator.Send(new GetExamQuery(examId, attemptId));

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(SaveAttemptAnswersCommand request)
        {
            var result = await Mediator.Send(request);

            return View();
        }
    }
}