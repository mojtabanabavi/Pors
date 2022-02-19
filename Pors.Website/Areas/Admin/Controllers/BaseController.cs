using System;
using MediatR;
using System.Linq;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Admin)]
    public class BaseController : Controller
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected DataTableRequest BindDataTableRequest()
        {
            var draw = Request.Form["draw"].FirstOrDefault();

            // skip number of rows count
            var start = Request.Form["start"].FirstOrDefault();

            // paging length
            var length = Request.Form["length"].FirstOrDefault();

            // sort column name
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            // sort column direction (asc, desc) 
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // search value from (search box) 
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            start = start == "0" ? "1" : start;
            int page = start != null ? Convert.ToInt32(start) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 15;

            var result = new DataTableRequest
            {
                Draw = draw,
                Page = page,
                PageSize = pageSize,
                SortColumn = sortColumn,
                Search = searchValue.ToLower(),
                SortColumnDirection = sortColumnDirection
            };

            return result;
        }
    }
}