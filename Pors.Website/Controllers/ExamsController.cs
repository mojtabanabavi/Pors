using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pors.Website.Controllers
{
    public class ExamsController : BaseController
    {
        [HttpGet]
        public IActionResult Start()
        {
            return View();
        }
    }
}