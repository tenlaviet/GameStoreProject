using AspMVC.Areas.Identity.Controllers;
using AspMVC.Areas.Main.Models;
using AspMVC.Data;
using AspMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        [Route("/main")]
        public async Task<IActionResult> Index()
        {
            var gamesCollumn = await _context.ProjectPages.Include(c=>c.ProjectCoverImage).Take(21).ToListAsync();
            
            return View(gamesCollumn);
        }
        [HttpGet("/browse/{genre?}")]
        public async Task<IActionResult> Browse(string? genreSlug)
        {
            List<GameCell> games = new List<GameCell>();
            List<Genre> genres = new List<Genre>();
            if (string.IsNullOrEmpty(genreSlug))
            {
                games = await _context.ProjectPages.Include(c => c.ProjectCoverImage).
                                                    Include(c => c.Creator).
                                                    Include(g => g.Genre).
                                                    Take(20).
                                                    Select(c =>
                                                    new GameCell
                                                    {
                                                        GameId = c.ProjectId,
                                                        Title = c.Title,
                                                        CreatorID = c.CreatorId,
                                                        CreatorName = c.Creator.UserName,
                                                        ShortDescription = c.ShortDescription,
                                                        GenreName = c.Genre.GenreName,
                                                        CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath
                                                    }).ToListAsync();
            }
            else
            {
                games = await _context.ProjectPages.Include(c => c.ProjectCoverImage).
                                                    Include(c => c.Creator).
                                                    Include(g => g.Genre).
                                                    Where(s => s.Genre.Slug == genreSlug).
                                                    Take(20).
                                                    Select(c =>
                                                    new GameCell
                                                    {
                                                        GameId = c.ProjectId,
                                                        Title = c.Title,
                                                        CreatorID = c.CreatorId,
                                                        CreatorName = c.Creator.UserName,
                                                        ShortDescription = c.ShortDescription,
                                                        GenreName = c.Genre.GenreName,
                                                        CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath
                                                    }).ToListAsync();
            }

            genres = await _context.Genres.ToListAsync();

            BrowseViewModel vm = new BrowseViewModel()
            {
                gameCells = games,
                Genres = genres
            };
            return View(vm);



        }
        [Route("/search")]
        public async Task<IActionResult> Search(string? search, string? genre, string? when)
        {
            if (_context == null)
            {
                return Problem("Entity set  is null.");
            }
            var games = _context.ProjectPages.Include(c => c.ProjectCoverImage).Take(21);


            
            if (!String.IsNullOrEmpty(search))
            {
                games = games.Where(s => s.Title.Contains(search));
                ViewData["q"] = search;
                return View(await games.ToListAsync());
            }
            return RedirectToAction("Index");
        }
    }
}
