using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
