using System;
using MediatR;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Website.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Management)]
    public class BaseController : Controller
    {
        private ISender _mediator = null!;
        private IDataTableService _dataTable = null!;

        protected ISender Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IDataTableService DataTable =>
            _dataTable ??= HttpContext.RequestServices.GetRequiredService<IDataTableService>();
    }
}