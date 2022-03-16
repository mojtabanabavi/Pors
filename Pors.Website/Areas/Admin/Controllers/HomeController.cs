using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Areas.Admin.Controllers
{
    [DisplayName("داشبورد")]
    public class HomeController : BaseController
    {
        [HttpGet]
        [DisplayName("مشاهده‌ی گزارشات")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
