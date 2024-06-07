using AspMVC.Data;
using AspMVC.Models;
using AspMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace AspMVC.Areas.Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
       // private readonly IComment _commentService;
        public CommentController(AppDbContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("/SendComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentViewModel commentForm)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Comment comment = new Comment()
            {
                CommentContent = commentForm.Content,
                TimeStamp = DateTime.Now,
                AuthorId = currentUserID,
                ProjectPageId = commentForm.PageId,
                //ParentCommentId = parentCommentId,
                IsDeleted = false
            };


            await _context.Comments.AddAsync(comment);
            try
            {
                await _context.SaveChangesAsync();
                var addedComment = await _context.Comments.Where(c => c.CommentId == comment.CommentId).Include(a => a.Author).FirstOrDefaultAsync();

                var addedCommentVM = new CommentCardViewModel()
                {
                    LastSent = "a few seconds ago",
                    AuthorName = addedComment.Author.UserName,
                    Content = addedComment.CommentContent
                };
                return PartialView("~/Areas/Blog/Views/Shared/_CommentCard.cshtml", addedCommentVM);
                //return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false });
            }
            //return new JsonResult(Ok());
        }
        [HttpGet("GetComments/{pageID}")]
        public async Task<IActionResult> GetComments(int pageID)
        {
            //var result = await _context.Comments.FirstOrDefaultAsync();
            //var results = await _context.Comments.Where(p => p.ProjectPageId == pageID).Include(a => a.Author).OrderByDescending(c => c.CommentId).ToListAsync();
            //var result = new CommentCardViewModel()
            //{
            //    LastSent = "just now",
            //    AuthorName = ,
            //    Content = "test"
            //};

            //return Json(new { Data = PartialViewResults, TotalItems = PartialViewResults.Count });
            //return PartialView("~/Areas/Blog/Views/Shared/_CommentCard.cshtml", result);
            return Json(Ok());
        }
        [HttpPost("/EditComment")]
        public async Task<IActionResult> EditComment(int commentID, string content)
        {
            var com = await _context.Comments.FindAsync(commentID);
            if(com != null)
            {
                com.CommentContent = content;
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DetleteComment(int commentID)
        {
            return Json(new { commentID });
        }

    }
}
