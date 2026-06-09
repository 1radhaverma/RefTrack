using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class JobRolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
