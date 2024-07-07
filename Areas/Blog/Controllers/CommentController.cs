using AspMVC.ViewModels;
using AspMVC.Data;
using AspMVC.Models;
using AspMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Security.Claims;
using AspMVC.Migrations;
using AspMVC.Models.EF;

namespace AspMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
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
            Comment newComment = new Comment()
            {
                CommentContent = commentForm.Content,
                TimeStamp = DateTime.Now,
                AuthorId = currentUserID,
                ProjectPageId = commentForm.PageId,
                //ParentCommentId = parentCommentId,
                IsDeleted = false
            };


            await _context.Comments.AddAsync(newComment);
            try
            {
                await _context.SaveChangesAsync();
                var addedComment = await _context.Comments.Where(c => c.CommentId == newComment.CommentId).Include(a => a.Author).FirstOrDefaultAsync();

                var addedCommentVM = new CommentCardViewModel()
                {
                    CommentID = addedComment.CommentId,
                    LastSent = "a few seconds ago",
                    AuthorName = addedComment.Author.UserName,
                    Content = addedComment.CommentContent,
                    isAuthor = true
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

        [HttpPost("/EditComment")]
        public async Task<IActionResult> EditComment(int commentID, string content)
        {
            var com = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentID);
            if(com != null)
            {
                com.CommentContent = content;
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentID)
        {
            var com = await _context.Comments.FindAsync(commentID); 
            if(com != null)
            {
                _context.Comments.Remove(com);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }

        [HttpPost()]
        public async Task<IActionResult> RateProject(RatingSectionViewModel result)
        {
            
            var currentUserRating = await _context.ProjectRatings
               .Where(p => p.ProjectPageId == result.PageId)
               .Where(p => p.UserId == result.UserId).FirstOrDefaultAsync();
            if(currentUserRating != null)
            {
                currentUserRating.RatingScore = result.RatingScore;
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            
            ProjectRating rate = new ProjectRating()
            {
                ProjectPageId = result.PageId,
                UserId = result.UserId,
                RatingScore = result.RatingScore,
            };
            _context.ProjectRatings.Add(rate);
            await _context.SaveChangesAsync();
            
            return Json(new { success = true });

        }

    }
}
