using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pors.Application.Management.Permissions.Queries;
using Pors.Application.Management.Permissions.Commands;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت دسترسی")]
    public class PermissionController : BaseController
    {
        [HttpGet]
        [DisplayName("ویرایش دسترسی")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public async Task<IActionResult> Update(GetRolePermissionsQuery request)
        {
            if (request.Id == default(int))
            {
                return View();
            }

            var result = await Mediator.Send(request);

            return View(result);
        }

        [HttpPost]
        [DisplayName("ویرایش دسترسی")]
        public async Task<IActionResult> Update(UpdatePermissionsCommand request)
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(request);
            }

            var permissions = await Mediator.Send(new GetRolePermissionsQuery(request.RoleId));

            return View(permissions);
        }
    }
}