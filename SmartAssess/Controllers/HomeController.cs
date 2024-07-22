using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
