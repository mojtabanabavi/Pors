using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Users.Queries;
using Pors.Application.Management.Users.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> GetUsers()
        {
            var query = DataTable.FetchRequest();

            var request = new GetUsersQuery(query);

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
                var userId = await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetUserQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var user = await Mediator.Send(new GetUserQuery(request.Id));

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteUserCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
