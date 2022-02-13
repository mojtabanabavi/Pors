using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
