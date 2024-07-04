using AspMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AspMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminPanelController : Controller
    {
        [HttpGet("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
