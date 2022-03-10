using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Pors.Application.Management.Roles.Queries;
using Pors.Application.Management.Roles.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> GetRoles()
        {
            var query = DataTable.FetchRequest();

            var request = new GetRolesQuery(query);

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
        public async Task<IActionResult> Create(CreateRoleCommand request)
        {
            if (ModelState.IsValid)
            {
                var roleId = await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(GetRoleQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);

                return RedirectToAction(nameof(Index));
            }

            var role = await Mediator.Send(new GetRoleQuery(request.Id));

            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(DeleteRoleCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
