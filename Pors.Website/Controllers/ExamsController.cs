using System;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Public.Exams.Queries;
using Pors.Application.Public.Exams.Commands;
using Pors.Application.Public.Reports.Queries;
using Pors.Application.Public.Answers.Commands;

namespace Pors.Website.Controllers
{
    public class ExamsController : BaseController
    {
        #region api

        [HttpPost]
        public async Task<IActionResult> GetQuestionAnswersChartData(GetAnswersChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetQuestionAnswersAccuracyChartData(GetAnswersAccuracyChartDataQuery request)
        {
            var result = await Mediator.Send(request);

            return Json(result);
        }

        #endregion;

        [HttpGet]
        public async Task<IActionResult> Index(GetExamsQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(GetExamDetailsQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Attempt(int id)
        {
            var participantId = HttpContext.Request.Cookies[ParticipantCookieKeys.ParticipantId].ToString();

            var attemptId = await Mediator.Send(new CreateExamAttemptCommand(id, participantId));

            return RedirectToAction(nameof(Start), new { AttemptId = attemptId });
        }

        [HttpGet("exams/start/{attemptId}")]
        public async Task<IActionResult> Start(string attemptId)
        {
            var result = await Mediator.Send(new GetExamForAttemptQuery(attemptId));

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(SaveExamAnswersCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsSucceeded)
            {
                var attemptId = result.Data;

                return RedirectToAction(nameof(Result), new { AttemptId = attemptId });
            }

            ModelState.AddErrors(result.Errors);

            var exam = await Mediator.Send(new GetExamForAttemptQuery(request.AttemptId));

            return View(nameof(Start), exam);
        }

        [HttpGet("exams/result/{attemptId}")]
        public async Task<IActionResult> Result(GetExamResultQuery request)
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