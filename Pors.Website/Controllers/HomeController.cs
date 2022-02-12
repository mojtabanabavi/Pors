using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
