using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
