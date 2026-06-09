using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class ReferrersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
