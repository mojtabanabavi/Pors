using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("داشبورد")]
    public class DashboardController : BaseController
    {
        [HttpGet]
        [DisplayName("مشاهده‌ی گزارشات")]
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