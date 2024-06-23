using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspMVC.Data;
using AspMVC.Models;
using Microsoft.AspNetCore.Identity;
using AspMVC.Areas.Identity.Controllers;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;
using AspMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.Design;
using AspMVC.Services;
using AspMVC.Models.EF;
using System.Xml.Linq;

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
        

        // GET: Blog/ProjectPage
        public async Task<IActionResult> Index()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var appDbContext = _context.ProjectPages.Where(u => u.CreatorId == userid).Include(p => p.Genre);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Blog/ProjectPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }
            //var query = await (from p in _context.ProjectPages
            //             join g in _context.Genres on p.GenreId equals g.Id
            //             join c in _context.Comments on p.Id equals c.ProjectPageId
            //             join u in _context.Users on c.UserId equals u.Id
            //             select new
            //             {
            //                 pageID = p.Id,
            //                 projectTitle = p.Title,
            //                 projectDescription = p.Description,
            //                 projectGenre = p.Genre,
            //                 pageComments = p.Comments,
            //                 commentAuthor = u.UserName
            //             }).FirstOrDefaultAsync();

            var projectPageModel = await _context.ProjectPages
                .Include(p => p.Genre).Include(c => c.Comments).Include(f =>f.ProjectFiles).Include(p => p.ProjectPictures)
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            var pageCommentsList = await _context.Comments
                .Where(cp => cp.ProjectPageId == id)
                .Include(u => u.Author)
                .OrderByDescending(t => t.TimeStamp)
                .ToListAsync();

            if (projectPageModel == null)
            {
                return NotFound();
            }
            CommentSectionViewModel commentSectionViewModel = new CommentSectionViewModel()
            {
                PageId = id,
                Comments = pageCommentsList
            };
            ProjectPageDetailViewModel detailViewModel = new ProjectPageDetailViewModel()
            {
                ProjectPage = projectPageModel,
                CommentSection = commentSectionViewModel

            };


            return View(detailViewModel);
        }
        [Authorize]
        // GET: Blog/ProjectPage/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
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
                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.Identity.Name;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                ///////////////them ban ghi projectpage moi vao database//////////////////////
                ProjectPageModel projectpage = new ProjectPageModel
                {
                    CreatorId = currentUserID,
                    Title = projectPageModel.Title,
                    ShortDescription = projectPageModel.ShortDescription,
                    Description = projectPageModel.Description,
                    GenreId = projectPageModel.GenreId,
                    Slug = projectPageModel.Slug
                };

                //save  project page moi vao database
                await _context.ProjectPages.AddAsync(projectpage);

                await _context.SaveChangesAsync();
                //////////////////////////////////////////////////

                //xu ly upload file len server va them du lieu vao sql
                var uploadedProjectPictureList = projectPageModel.PictureUpload;
                var uploadedProjectFileList = projectPageModel.FileUpload;

                //uploaded file directories
                var projectImageStorageDir = Path.Combine(_environment.WebRootPath, "uploads", "Users", currentUserName, "Projects", projectPageModel.Title);
                var projectImageGalleryStorageDir = Path.Combine(projectImageStorageDir, "Screenshots");
                var projectCoverImageDir = Path.Combine(projectImageStorageDir, "CoverImage");
                var projectFileStorageDir = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", currentUserName, "Projects", projectPageModel.Title);

                Directory.CreateDirectory(projectImageStorageDir);
                Directory.CreateDirectory(projectImageGalleryStorageDir);
                Directory.CreateDirectory(projectCoverImageDir);
                Directory.CreateDirectory(projectFileStorageDir);

                //xu ly upload gallery

                if (projectPageModel.PictureUpload != null)
                {
                    foreach (var file in projectPageModel.PictureUpload)
                    {
                        string uploadedFile = Path.Combine(projectImageGalleryStorageDir, file.FileName);
                        using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/Screenshots/{file.FileName}";
                        ProjectUploadedPicture addUploadedPicture = new ProjectUploadedPicture()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectPicture = uploadedFile,
                            ProjectPictureRelativePath = fileRelativePath
                        };
                        await _context.ProjectUploadedPicture.AddAsync(addUploadedPicture);

                    }
                }
                //

                //xu ly upload coverimage
                if (projectPageModel.CoverPictureUpload != null)
                {
                        var file = projectPageModel.CoverPictureUpload;
                        string uploadedFile = Path.Combine(projectCoverImageDir, file.FileName);
                        using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/CoverImage/{file.FileName}";
                        ProjectUploadedCoverImage addUploadedPicture = new ProjectUploadedCoverImage()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectCoverImage = uploadedFile,
                            ProjectCoverImageRelativePath = fileRelativePath
                        };
                        await _context.ProjectUploadedCoverImage.AddAsync(addUploadedPicture);

                }

                //

                // xu ly upload file
                if (projectPageModel.FileUpload != null)
                {
                    foreach (var file in projectPageModel.FileUpload)
                    {
                        string uploadedFile = Path.Combine(projectFileStorageDir, file.FileName);
                        using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        ProjectUploadedFile addUploadedFile = new ProjectUploadedFile()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectFile = uploadedFile,
                        };
                        await _context.ProjectUploadedFile.AddAsync(addUploadedFile);
                    }
                }
                //
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

            var projectPage = await _context.ProjectPages.FindAsync(id);
            if (projectPage == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPage.GenreId);

            EditProjectPageViewModel viewModel = new EditProjectPageViewModel()
            {
                ProjectID = projectPage.ProjectId,
                Title = projectPage.Title,
                Description = projectPage.Description,
                ShortDescription = projectPage.ShortDescription,
                Slug = projectPage.Slug,
                GenreId = projectPage.GenreId
            };
            return View(viewModel);

        }

        // POST: Blog/ProjectPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProjectPageViewModel projectPageModel)
        {
            
            if (ModelState.IsValid)
            {
                var project = await _context.ProjectPages.FirstOrDefaultAsync(p => p.ProjectId == projectPageModel.ProjectID);
                if (project != null)
                {
                    project.Title = projectPageModel.Title;
                    project.ShortDescription = projectPageModel.ShortDescription;
                    project.Description = projectPageModel.Description;
                    project.Slug = projectPageModel.Slug;
                    project.GenreId = projectPageModel.GenreId;

                }
                await _context.SaveChangesAsync();
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
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        // POST: Blog/ProjectPage/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.ProjectPages == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.ProjectPages'  is null.");
        //    }
        //    var projectPageModel = await _context.ProjectPages.FindAsync(id);
        //    //var projectPageModel = await _context.ProjectPages.Where(x => x.Id == id).Include(c => c.Comments).FirstOrDefaultAsync();
        //    if (projectPageModel != null)
        //    {

        //        //var FileStoragePath = Path.GetDirectoryName(projectPageModel.ProjectFileDirectory);

        //        if (Directory.Exists(FileStoragePath))
        //        {
        //            //xoa' folder chua' project va tat ca cac file va folder ben trong
        //            DirectoryInfo dir = new DirectoryInfo(FileStoragePath);
        //            dir.Delete(true);
        //        }

        //        _context.ProjectPages.Remove(projectPageModel);
        //        await _context.SaveChangesAsync();
        //    }





        //    return RedirectToAction(nameof(Index));
        //}

        private bool ProjectPageModelExists(int id)
        {
            return (_context.ProjectPages?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }

        //public async Task<IActionResult> Download(int? id)
        //{
        //    if (id == null || _context.ProjectPages == null)
        //    {
        //        return NotFound();
        //    }

        //    var projectPageModel = await _context.ProjectPages
        //                                .FirstOrDefaultAsync(m => m.ProjectId == id);
        //    if (projectPageModel == null)
        //    {
        //        return NotFound();
        //    }
        //    string DefaultContentType = "application/octet-stream";
        //    string FilePath = projectPageModel.ProjectFileDirectory;
        //    var provider = new FileExtensionContentTypeProvider();

        //    var fileExists = System.IO.File.Exists(FilePath);
        //    //doc noi dung file 
        //    byte[] FileContent = System.IO.File.ReadAllBytes(FilePath);
        //    string fileName = Path.GetFileName(FilePath);


        //    if (!provider.TryGetContentType(FilePath, out string contentType))
        //    {
        //        contentType = DefaultContentType;
        //    }

        //    return File(FileContent, contentType, fileName);

        //}



    }
}
