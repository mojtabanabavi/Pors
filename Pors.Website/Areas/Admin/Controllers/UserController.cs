using System;
using System.ComponentModel;
using Pors.Website.Constants;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pors.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Users.Queries;
using Pors.Application.Management.Users.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت کاربران")]
    public class UserController : BaseController
    {
        #region api

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

        #endregion;

        [HttpGet]
        [DisplayName("لیست کاربران")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [DisplayName("ایجاد کاربر")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DisplayName("ایجاد کاربر")]
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
        [DisplayName("ویرایش کاربر")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Update(GetUserQuery request)
        {
            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش کاربر")]
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
        [DisplayName("حذف کاربر")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
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
