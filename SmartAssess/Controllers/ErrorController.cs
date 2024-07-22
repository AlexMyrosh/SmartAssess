using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error400()
        {
            Response.StatusCode = 400;
            return View("400");
        }

        public IActionResult Error401()
        {
            Response.StatusCode = 401;
            return View("401");
        }

        public IActionResult Error403()
        {
            Response.StatusCode = 403;
            return View("403");
        }

        [HttpGet("/Error/404")]
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("404");
        }

        [HttpGet("/Error/500")]
        public IActionResult Error5xx()
        {
            Response.StatusCode = 500;
            return View("500");
        }
    }
}
