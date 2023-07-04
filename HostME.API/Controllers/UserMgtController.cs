using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{
    public class UserMgtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
