using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
