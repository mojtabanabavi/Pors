using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
