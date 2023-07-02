using Microsoft.AspNetCore.Mvc;

namespace Hostel.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
