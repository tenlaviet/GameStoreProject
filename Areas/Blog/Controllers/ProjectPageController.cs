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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Humanizer;

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
            var indexModel = _context.ProjectPages.Where(u => u.CreatorId == userid).Include(p => p.Genre).Include(c=>c.ProjectCoverImage);
            return View(await indexModel.ToListAsync());
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

            var projectPage = await _context.ProjectPages
                .Include(p => p.Genre).Include(c => c.Comments).Include(f =>f.ProjectFiles).Include(p => p.ProjectPictures)
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            var pageCommentsList = await _context.Comments
                .Where(cp => cp.ProjectPageId == id)
                .Include(u => u.Author)
                .OrderByDescending(t => t.TimeStamp)
                .ToListAsync();

            if (projectPage == null)
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
                ProjectPage = projectPage,
                CommentSection = commentSectionViewModel

            };
            projectPage.ViewCount++;
            _context.SaveChanges();

            return View(detailViewModel);
        }
        [Authorize]
        // GET: Blog/ProjectPage/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
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


                var projectImageStorageDir = Path.Combine(_environment.WebRootPath, "uploads", "Users", currentUserName, "Projects", projectPageModel.Title);
                var projectFileStorageDir = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", currentUserName, "Projects", projectPageModel.Title);

                ///////////////them ban ghi projectpage moi vao database//////////////////////
                ProjectPageModel projectpage = new ProjectPageModel
                {
                    CreatorId = currentUserID,
                    Title = projectPageModel.Title,
                    ShortDescription = projectPageModel.ShortDescription,
                    Description = projectPageModel.Description,
                    GenreId = projectPageModel.GenreId,
                    PlatformId = projectPageModel.PlatformId,
                    Slug = projectPageModel.Slug,
                    ProjectPageDatePosted = DateTime.Now,
                    ProjectFilesDir = projectFileStorageDir,
                    ProjectImagesDir = projectImageStorageDir
                    
                    
                    
                };

                //save  project page moi vao database
                await _context.ProjectPages.AddAsync(projectpage);

                await _context.SaveChangesAsync();
                //////////////////////////////////////////////////

                //xu ly upload file len server va them du lieu vao sql
                //var uploadedProjectPictureList = projectPageModel.PictureUpload;
                //var uploadedProjectFileList = projectPageModel.FileUpload;

                //uploaded file directories
                var projectImageGalleryStorageDir = Path.Combine(projectImageStorageDir, "Screenshots");
                var projectCoverImageDir = Path.Combine(projectImageStorageDir, "CoverImage");

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
                        using (var fileStream = System.IO.File.Create(uploadedFile))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/Screenshots/{file.FileName}";
                        ProjectUploadedPicture addUploadedPicture = new ProjectUploadedPicture()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectPicture = uploadedFile,
                            ProjectPictureRelativePath = fileRelativePath,
                            PictureName = file.FileName
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
                        using (var fileStream = System.IO.File.Create(uploadedFile))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/CoverImage/{file.FileName}";
                        ProjectUploadedCoverImage addUploadedPicture = new ProjectUploadedCoverImage()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectCoverImage = uploadedFile,
                            ProjectCoverImageRelativePath = fileRelativePath,
                            CoverName = file.FileName
                        };
                        await _context.ProjectUploadedCoverImage.AddAsync(addUploadedPicture);

                }

                //

                // xu ly upload file
                if (projectPageModel.FileUpload != null)
                {
                    foreach (var file in projectPageModel.FileUpload)
                    {
                        string size = file.Length.Bytes().Humanize();
                        string uploadedFile = Path.Combine(projectFileStorageDir, file.FileName);
                        using (var fileStream = System.IO.File.Create(uploadedFile))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        ProjectUploadedFile addUploadedFile = new ProjectUploadedFile()
                        {
                            ProjectPageID = projectpage.ProjectId,
                            ProjectFile = uploadedFile,
                            FileName = file.FileName,
                            FileSize = size
                        };
                        await _context.ProjectUploadedFile.AddAsync(addUploadedFile);
                    }
                }
                //
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", projectPageModel.GenreId);
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", projectPageModel.GenreId);

            return View(projectPageModel);


        }

        // GET: Blog/ProjectPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var projectPage = await _context.ProjectPages.Include(p => p.ProjectPictures).Include(p => p.ProjectCoverImage).Include(p => p.ProjectFiles).FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectPage == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", projectPage.GenreId);
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", projectPage.PlatformId);

            //ViewData["Gallery"] = new MultiSelectList(_context.ProjectUploadedPicture.Where(p=>p.ProjectPageID == id), "PictureID", "PictureName");

            //var projectFiles = await _context.ProjectUploadedFile.Where(p => p.ProjectPageID == id).ToListAsync();
            ViewData["RemoveFiles"] = new MultiSelectList(projectPage.ProjectFiles, "FileID", "FileName");

            //var pictureIDList = await _context.ProjectUploadedPicture.Where(i => i.ProjectPageID == id).Select(i => i.PictureID).ToListAsync();
            var pictureIDList = projectPage.ProjectPictures.Select(i=>i.PictureID).ToList();
            var removePictureList = new List<RemovePictureCheckBox>();
            RemovePictureCheckBox removeCover = null;
            if (projectPage.ProjectCoverImage!=null)
            {
                removeCover = new RemovePictureCheckBox() { PictureID = projectPage.ProjectCoverImage.CoverID, Selected = false };

            }

            foreach (var pictureID in pictureIDList)
            {
                removePictureList.Add(new RemovePictureCheckBox { PictureID = pictureID, Selected = false });
            }
            
            EditProjectPageViewModel viewModel = new EditProjectPageViewModel()
            {
                ProjectID = projectPage.ProjectId,
                Title = projectPage.Title,
                Description = projectPage.Description,
                ShortDescription = projectPage.ShortDescription,
                Slug = projectPage.Slug,
                GenreId = projectPage.GenreId,
                PlatformId = projectPage.PlatformId,
                ProjectGallery = projectPage.ProjectPictures.ToList(),
                RemoveGallery = removePictureList,
                ProjectCover = projectPage.ProjectCoverImage,
                RemoveCover = removeCover,

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
                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.Identity.Name;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                var project = await _context.ProjectPages.Include(p => p.ProjectPictures).Include(p => p.ProjectCoverImage).Include(p => p.ProjectFiles).FirstOrDefaultAsync(p => p.ProjectId == projectPageModel.ProjectID);
                if (project != null)
                {
                    project.Title = projectPageModel.Title;
                    project.ShortDescription = projectPageModel.ShortDescription;
                    project.Description = projectPageModel.Description;
                    project.Slug = projectPageModel.Slug;
                    project.GenreId = projectPageModel.GenreId;
                    project.PlatformId = projectPageModel.PlatformId;
                }
                var pictureIdToRemoveList = projectPageModel.RemoveGallery;
                var coverIdToRemove = projectPageModel.RemoveCover;
                ////https://stackoverflow.com/questions/16824510/select-multiple-records-based-on-list-of-ids-with-linq
                //remove picture from database and server
                if(pictureIdToRemoveList != null && pictureIdToRemoveList.Count > 0)
                {
                    var pictureList = projectPageModel.RemoveGallery.Where(s => s.Selected == true).Select(i => i.PictureID).ToList();
                    var removePictureList = await _context.ProjectUploadedPicture.Where(e => pictureList.Contains(e.PictureID)).ToListAsync();
                    foreach(var picture in removePictureList)
                    {
                        try
                        {
                            if (System.IO.File.Exists(picture.ProjectPicture))
                            {
                                System.IO.File.Delete(picture.ProjectPicture);
                            }
                            else Console.WriteLine("File not found");
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }

                    }

                    _context.ProjectUploadedPicture.RemoveRange(removePictureList);
                }
                //remove file from database and server
                if(projectPageModel.RemoveFileIDs !=null && projectPageModel.RemoveFileIDs.Count > 0)
                {
                    var removeFileList = await _context.ProjectUploadedFile.Where(e => projectPageModel.RemoveFileIDs.Contains(e.FileID)).ToListAsync();
                    foreach (var file in removeFileList)
                    {
                        try
                        {
                            if (System.IO.File.Exists(file.ProjectFile))
                            {
                                System.IO.File.Delete(file.ProjectFile);
                            }
                            else Console.WriteLine("File not found");
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }

                    }
                    _context.ProjectUploadedFile.RemoveRange(removeFileList);
                }
                //remove cover from database and server
                if(coverIdToRemove != null && coverIdToRemove.Selected == true)
                {
                    var removeCover = await _context.ProjectUploadedCoverImage.FirstOrDefaultAsync(e => e.CoverID == coverIdToRemove.PictureID);
                    if (removeCover != null)
                    {
                        _context.ProjectUploadedCoverImage.Remove(removeCover);
                        try
                        {
                            if (System.IO.File.Exists(removeCover.ProjectCoverImage))
                            {
                                System.IO.File.Delete(removeCover.ProjectCoverImage);
                            }
                            else Console.WriteLine("File not found");
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                //add new pictures to database and server
                if (projectPageModel.PictureUpload != null && projectPageModel.PictureUpload.Count > 0)
                {
                    foreach (var file in projectPageModel.PictureUpload)
                    {
                        string projectImageGalleryStorageDir = Path.Combine(project.ProjectImagesDir, "Screenshots");
                        string uploadedFile = Path.Combine(projectImageGalleryStorageDir, file.FileName);
                        using (var fileStream = System.IO.File.Create(uploadedFile))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/Screenshots/{file.FileName}";
                        ProjectUploadedPicture addUploadedPicture = new ProjectUploadedPicture()
                        {
                            ProjectPageID = project.ProjectId,
                            ProjectPicture = uploadedFile,
                            ProjectPictureRelativePath = fileRelativePath,
                            PictureName = file.FileName
                        };
                        await _context.ProjectUploadedPicture.AddAsync(addUploadedPicture);

                    }
                }
                //add new cover to database and server
                if (projectPageModel.CoverPictureUpload != null)
                {
                    var file = projectPageModel.CoverPictureUpload;

                    string projectCoverImageDir = Path.Combine(project.ProjectImagesDir, "CoverImage");
                    string uploadedFile = Path.Combine(projectCoverImageDir, file.FileName);
                    using (var fileStream = System.IO.File.Create(uploadedFile))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    string fileRelativePath = $"/uploads/Users/{currentUserName}/Projects/{projectPageModel.Title}/CoverImage/{file.FileName}";
                    ProjectUploadedCoverImage addUploadedPicture = new ProjectUploadedCoverImage()
                    {
                        ProjectPageID = project.ProjectId,
                        ProjectCoverImage = uploadedFile,
                        ProjectCoverImageRelativePath = fileRelativePath,
                        CoverName = file.FileName
                    };
                    if(project.ProjectCoverImage !=null)
                    {
                        _context.ProjectUploadedCoverImage.Remove(project.ProjectCoverImage);
                    }
                    await _context.ProjectUploadedCoverImage.AddAsync(addUploadedPicture);

                }
                //add new file to database and server
                if (projectPageModel.FileUpload != null)
                {
                    foreach (var file in projectPageModel.FileUpload)
                    {
                        string projectFileStorageDir = project.ProjectFilesDir;
                        string uploadedFile = Path.Combine(projectFileStorageDir, file.FileName);
                        using (var fileStream = System.IO.File.Create(uploadedFile))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        ProjectUploadedFile addUploadedFile = new ProjectUploadedFile()
                        {
                            ProjectPageID = project.ProjectId,
                            ProjectFile = uploadedFile,
                            FileName = file.FileName
                        };
                        await _context.ProjectUploadedFile.AddAsync(addUploadedFile);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = project.ProjectId.ToString() });

            }
            else
            {
                //de phong xay ra loi model, xu ly sau nay
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            ViewData["PlatformId"] = new SelectList(_context.Platform, "PlatformId", "PlatformName", projectPageModel.GenreId);
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
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        //POST: Blog/ProjectPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectPageModel = await _context.ProjectPages.Include(p => p.ProjectPictures).Include(p => p.ProjectCoverImage).Include(p => p.ProjectFiles).FirstOrDefaultAsync(m => m.ProjectId == id);

            if (projectPageModel == null)
            {
                return NotFound();
            }


            var FileStorageDir = projectPageModel.ProjectFilesDir;
            var ImageStorageDir = projectPageModel.ProjectImagesDir;
            if (Directory.Exists(FileStorageDir))
            {
                //xoa' folder chua' project va tat ca cac file va folder ben trong
                DirectoryInfo dir = new DirectoryInfo(FileStorageDir);
                dir.Delete(true);
            }
            if (Directory.Exists(ImageStorageDir))
            {
                //xoa' folder chua' project va tat ca cac file va folder ben trong
                DirectoryInfo dir = new DirectoryInfo(ImageStorageDir);
                dir.Delete(true);
            }

            _context.ProjectPages.Remove(projectPageModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectPageModelExists(int id)
        {
            return (_context.ProjectPages?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Download(int? id)
        {
            if (id == null || _context.ProjectPages == null)
            {
                return NotFound();
            }

            var file = await _context.ProjectUploadedFile.FirstOrDefaultAsync(m => m.FileID == id);
            if (file == null)
            {
                return NotFound();
            }
            string DefaultContentType = "application/octet-stream";
            string FilePath = file.ProjectFile;
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
