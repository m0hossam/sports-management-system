using Microsoft.AspNetCore.Mvc;

namespace SportsWebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
