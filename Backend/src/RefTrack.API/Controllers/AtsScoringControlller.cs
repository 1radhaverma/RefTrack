using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class AtsScoringControlller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
