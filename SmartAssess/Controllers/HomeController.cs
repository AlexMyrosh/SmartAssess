using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
