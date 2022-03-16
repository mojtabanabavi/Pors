using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("مدیریت دسترسی")]
    public class PermissionController : BaseController
    {
        [DisplayName("ویرایش دسترسی")]
        public IActionResult Update(int id)
        {
            return View();
        }
    }
}