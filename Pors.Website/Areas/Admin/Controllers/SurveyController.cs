using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
