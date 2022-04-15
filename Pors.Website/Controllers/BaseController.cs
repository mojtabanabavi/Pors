using System;
using MediatR;
using Pors.Website.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Website.Controllers
{
    [SetParticipantId]
    public class BaseController : Controller
    {
        private ISender _mediator = null!;

        protected ISender Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}