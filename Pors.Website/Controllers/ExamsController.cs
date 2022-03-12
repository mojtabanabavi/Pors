using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Public.Exams.Queries;
using Pors.Application.Public.ExamAttempts.Queries;
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

        [HttpGet("exams/start/{examId}/{attemptId}")]
        public async Task<IActionResult> Start(int examId, string attemptId)
        {
            var result = await Mediator.Send(new GetExamQuery(examId, attemptId));

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(SaveAttemptAnswersCommand request)
        {
            var attemptId = await Mediator.Send(request);

            return RedirectToAction(nameof(Result), new { AttemptId = attemptId });
        }

        [HttpGet("exams/result/{attemptId}")]
        public async Task<IActionResult> Result(GetExamAttemptAnswersQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(UpdateAnswerCommentCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok();
        }
    }
}