using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
