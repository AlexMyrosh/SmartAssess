using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class TrashController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new TrashViewModel();
            return View(viewModel);
        }
    }
}
