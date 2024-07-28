using Microsoft.AspNetCore.Mvc;
using Business_Logic_Layer.Services.Interfaces;
using Presentation_Layer.ViewModels;

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
