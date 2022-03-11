using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Public.Exams.Queries;
using Pors.Application.Public.ExamVisits.Commands;

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
        public async Task<IActionResult> Start(GetExamQuery request)
        {
            await Mediator.Send(new CreateExamVisitCommand(request.Id));

            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(answer a)
        {


            return View();
        }
    }
}