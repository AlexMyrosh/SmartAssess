using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
