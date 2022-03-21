using System.ComponentModel;
using Pors.Website.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("داشبورد")]
    public class DashboardController : BaseController
    {
        [HttpGet]
        [DisplayName("مشاهده")]
        [Authorize(Policy = PolicyNames.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/admin/error")]
        public IActionResult Error(int statusCode)
        {
            return View("_errorPage", statusCode);
        }
    }
}