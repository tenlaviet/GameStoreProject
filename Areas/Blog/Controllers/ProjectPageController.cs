using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspMVC.Areas.Blog.Models.ProjectViewModels;
using Microsoft.EntityFrameworkCore;
using AspMVC.Data;
using AspMVC.Models;
using Microsoft.AspNetCore.Identity;
using AppMVC.Areas.Identity.Controllers;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;



namespace AspMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ProjectPageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public ProjectPageController(AppDbContext context,
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
        private string? uploadedFile;

        // GET: Blog/ProjectPage
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProjectPages.Include(p => p.Genre);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Blog/ProjectPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPages
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        // GET: Blog/ProjectPage/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            return View();
        }

        // POST: Blog/ProjectPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectPageViewModel projectPageModel)
        {
            
                if (ModelState.IsValid)
                {
                    if (projectPageModel.FileUpload != null)
                    {

                        var username = User.Identity.Name;
                        var FileStoragePath = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", "ProjectsFiles");
                        var userDir = Path.Combine(FileStoragePath, username);
                        var userProjectDir = Path.Combine(userDir, projectPageModel.Title);
                        if (!Directory.Exists(userDir))
                        {
                            Directory.CreateDirectory(userDir);
                        }
                        if (!Directory.Exists(userProjectDir))
                        {
                            Directory.CreateDirectory(userProjectDir);

                        }
                        uploadedFile = Path.Combine(userProjectDir, projectPageModel.FileUpload.FileName);
                        using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
                        {
                            await projectPageModel.FileUpload.CopyToAsync(fileStream);
                        }
                    }
                    ProjectPageModel projectpage = new ProjectPageModel(
                                    projectPageModel.Title,
                                    projectPageModel.ShortDescription,
                                    projectPageModel.Description,
                                    projectPageModel.GenreId,
                                    projectPageModel.Slug,
                                    uploadedFile);
                    await _context.ProjectPages.AddAsync(projectpage);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
                return View(projectPageModel);

            
        }

        // GET: Blog/ProjectPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPages.FindAsync(id);
            if (projectPageModel == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            return View(projectPageModel);

        }

        // POST: Blog/ProjectPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,GenreId")] ProjectPageModel projectPageModel)
        {
            if (id != projectPageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectPageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectPageModelExists(projectPageModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            return View(projectPageModel);
        }

        // GET: Blog/ProjectPage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPages
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        // POST: Blog/ProjectPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectPages == null)
            {
                return Problem("Entity set 'AppDbContext.ProjectPages'  is null.");
            }
            var projectPageModel = await _context.ProjectPages.FindAsync(id);
            if (projectPageModel != null)
            {
                _context.ProjectPages.Remove(projectPageModel);
            }

            await _context.SaveChangesAsync();

            var username = User.Identity.Name;
            var FileStoragePath = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", "ProjectsFiles", username, projectPageModel.Title);
            if(Directory.Exists(FileStoragePath))
            {
                //xoa' folder chua' project va tat ca cac file va folder ben trong
                DirectoryInfo dir = new DirectoryInfo(FileStoragePath);
                dir.Delete(true);
            }


            return RedirectToAction(nameof(Index));
        }

        private bool ProjectPageModelExists(int id)
        {
            return (_context.ProjectPages?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Download(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPages
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }
            string DefaultContentType = "application/octet-stream";
            string FilePath = projectPageModel.ProjectFileDirectory;
            var provider = new FileExtensionContentTypeProvider();
            
            var fileExists = System.IO.File.Exists(FilePath);
            //doc noi dung file 
            byte[] FileContent = System.IO.File.ReadAllBytes(FilePath);
            string fileName = Path.GetFileName(FilePath);


            if (!provider.TryGetContentType(FilePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return File(FileContent, contentType, fileName);

        }
        

    }
}
