using AspMVC.Areas.Identity.Controllers;
using AspMVC.Areas.Main.Models;
using AspMVC.Data;
using AspMVC.Models;
using AspMVC.Models.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
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
            var query = _context.ProjectPages.Include(c=>c.ProjectCoverImage).Include(c => c.Ratings);
            var featuredList = await query.OrderByDescending(c => c.ViewCount).Take(14).Select(c => new GameCell
            {
                ProjectId = c.ProjectId,
                Title = c.Title,
                CreatorID = c.CreatorId,
                CreatorName = c.Creator.UserName,
                ShortDescription = c.ShortDescription,
                GenreName = c.Genre.GenreName,
                CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                PlatformName = c.Platform.PlatformName,
            }).ToListAsync();
            
            var latestList = await query.OrderByDescending(c => c.ProjectPageDatePosted).Take(14).Select(c => new GameCell
            {
                ProjectId = c.ProjectId,
                Title = c.Title,
                CreatorID = c.CreatorId,
                CreatorName = c.Creator.UserName,
                ShortDescription = c.ShortDescription,
                GenreName = c.Genre.GenreName,
                CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                PlatformName = c.Platform.PlatformName,
            }).ToListAsync();
            var recentPopuplarList = await query.
                Where(x => x.ProjectPageDatePosted.CompareTo(DateTime.Now.AddDays(-30)) >= 0).
                OrderByDescending(c => c.ViewCount).
                Take(14).
                Select(c => new GameCell
            {
                ProjectId = c.ProjectId,
                Title = c.Title,
                CreatorID = c.CreatorId,
                CreatorName = c.Creator.UserName,
                ShortDescription = c.ShortDescription,
                GenreName = c.Genre.GenreName,
                CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                PlatformName = c.Platform.PlatformName,
            }).ToListAsync();
            var recentHighratedList = await query.Where(x => x.ProjectPageDatePosted.CompareTo(DateTime.Now.AddDays(-30)) >= 0)
                .Where(c=>c.Ratings.Count>0)
                .Take(14)
                .Select(c => new GameCell
                {
                    ProjectId = c.ProjectId,
                    Title = c.Title,
                    CreatorID = c.CreatorId,
                    CreatorName = c.Creator.UserName,
                    ShortDescription = c.ShortDescription,
                    GenreName = c.Genre.GenreName,
                    CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                    PlatformName = c.Platform.PlatformName,
                    AverageRating = c.Ratings.Average(c=>c.RatingScore)
                }).OrderByDescending(c=>c.AverageRating).ToListAsync();
            HomePageViewModel model = new HomePageViewModel()
            {
                featured = featuredList,
                lastest = latestList,
                recentHighrated = recentHighratedList,
                recentPopuplar = recentPopuplarList,
            };
            return View(model);
        }
        [HttpGet("/browse")]
        public async Task<IActionResult> Browse(string? sort, string? price, string? genre, string? when, string? platform)
        {
            if(_context.ProjectPages == null)
            {
                return Problem("Entity set 'gamestore.ProjectPages'  is null.");
            }
            bool isSort = !String.IsNullOrEmpty(sort);
            bool isPrice = !String.IsNullOrEmpty(price);
            bool isGenre = !String.IsNullOrEmpty(genre);
            bool isWhen = !String.IsNullOrEmpty(when);
            bool isPlatform = !String.IsNullOrEmpty(platform);


            ViewBag.Sort = isSort ? sort : "";
            ViewBag.Price = isPrice ? price : "";
            ViewBag.Genre = isGenre ? genre : "";
            ViewBag.When = isWhen ? when : "";
            ViewBag.Platform = isPlatform ? platform : "";
            List<Genre> genres = _context.Genres.OrderBy(g => g.GenreName).ToList();
            List<Platform> platforms = _context.Platform.OrderBy(g => g.PlatformName).ToList();
            

            var gameQuery = _context.ProjectPages.Include(c => c.ProjectCoverImage).
                                                    Include(c => c.Creator).
                                                    Include(g => g.Genre).Include(p => p.Platform).
                                                    Take(20);
            if (isSort)
            {
                switch (sort)
                {
                    case "most-popular":
                        gameQuery = gameQuery.OrderByDescending(v => v.ViewCount);
                        break;
                    case "most-recent":
                        gameQuery = gameQuery.OrderByDescending(v => v.ProjectPageDatePosted);
                        break;
                    case "top-rated":
                        gameQuery = gameQuery.Where(v=>v.Ratings.Count>0).OrderByDescending(v => v.Ratings.Average(v=>v.RatingScore));
                        break;
                    default:
                        gameQuery = gameQuery.OrderByDescending(v => v.ViewCount);
                        break;
                }
            }
            if (isGenre)
            {
                gameQuery = gameQuery.Where(x => x.Genre.GenreSlug == genre);
            }
            if (isWhen)
            {

                double? days;
                switch (when)
                {

                    case "last-day":
                        days = 1;
                        break;
                    case "last-7-days":
                        days = 7;
                        break;
                    case "last-30-days":
                        days = 30;
                        break;
                    default: days = null;
                        break;
                }
                if(days!=null)
                {
                    double value = days.Value;
                    gameQuery = gameQuery.Where(x => x.ProjectPageDatePosted.CompareTo(DateTime.Now.AddDays(-value)) >= 0);
                }

            }
            if (isPlatform)
            {
                gameQuery = gameQuery.Where(x => x.Platform.PlatformSlug == platform);
            }

            
            List<GameCell> games = await gameQuery.Select(c => new GameCell
            {
                ProjectId = c.ProjectId,
                Title = c.Title,
                CreatorID = c.CreatorId,
                CreatorName = c.Creator.UserName,
                ShortDescription = c.ShortDescription,
                GenreName = c.Genre.GenreName,
                CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                PlatformName = c.Platform.PlatformName,
            }).ToListAsync();

            BrowseViewModel vm = new BrowseViewModel()
            {
                GameCells = games,
                Genres = genres,
                Platforms = platforms
            };
            return View(vm);



        }
        [Route("/search")]
        public async Task<IActionResult> Search(string? q)
        {
            if (_context == null)
            {
                return Problem("Entity set  is null.");
            }
            var gamesQuery = _context.ProjectPages.Include(c => c.ProjectCoverImage).Take(21);


            
            if (!String.IsNullOrEmpty(q))
            {
                gamesQuery = gamesQuery.Where(s => s.Title.Contains(q));

                List<GameCell> games = await gamesQuery.Select(c => new GameCell
                {
                    ProjectId = c.ProjectId,
                    Title = c.Title,
                    CreatorID = c.CreatorId,
                    CreatorName = c.Creator.UserName,
                    ShortDescription = c.ShortDescription,
                    GenreName = c.Genre.GenreName,
                    CoverImage = c.ProjectCoverImage.ProjectCoverImageRelativePath,
                    PlatformName = c.Platform.PlatformName,
                }).ToListAsync();
                ViewData["q"] = q;
                return View(games);
            }
            return RedirectToAction("Index");
        }
    }
}
