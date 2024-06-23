using AspMVC.Areas.Identity.Controllers;
using AspMVC.Data;
using AspMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspMVC.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomePageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        private string? uploadedFile;
        public HomePageController(AppDbContext context,
            IWebHostEnvironment environment,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }
        [Route("main")]
        public async Task<IActionResult> Index()
        {
            var gamesCollumn = await _context.ProjectPages.Include(c=>c.ProjectCoverImage).Take(21).ToListAsync();
            
            return View(gamesCollumn);
        }
    }
}
