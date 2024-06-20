using Microsoft.AspNetCore.Mvc;

namespace AspMVC.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomePageController : Controller
    {
        [Route("main")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
