using Microsoft.AspNetCore.Mvc;

namespace SportsWebApp.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SystemAdminRegister()
        {
            return View();
        }
    }
}
