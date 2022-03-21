using System;
using Microsoft.AspNetCore.Mvc;

namespace Pors.Website.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/error")]
        public IActionResult Error(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;

            switch (statusCode)
            {
                case 404: return View("_notFound");
                case 500: return View("_serverError");
            }

            return View();
        }
    }
}