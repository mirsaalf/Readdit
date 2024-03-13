using Microsoft.AspNetCore.Mvc;

namespace Readdit.Controllers
{
    public class MembersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }
    }
}
