using System.Threading.Tasks;
using Pors.Website.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Users.Queries;
using Pors.Application.Users.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> GetUsers()
        {
            var dataTableRequest = BindDataTableRequest();

            var request = new GetUsersQuery
            {
                Page = dataTableRequest.Page,
                Search = dataTableRequest.Search,
                PageSize = dataTableRequest.PageSize,
                SortColumn = dataTableRequest.SortColumn,
                SortColumnDirection = dataTableRequest.SortColumnDirection
            };

            var result = await Mediator.Send(request);

            var jsonData = new DataTableResponse
            {
                Data = result.Items,
                Draw = dataTableRequest.Draw,
                RecordsTotal = result.TotalItems,
                RecordsFiltered = result.TotalItems,
            };

            return Json(jsonData);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetUserQuery request)
        {
            var result = await Mediator.Send(request);

            if (result.IsSucceeded)
            {
                return View(result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteUserCommand request)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(request);

                if (!result.IsSucceeded)
                {
                    ModelState.AddErrors(result.Errors);

                    //return View(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index));
        }
    }
}
