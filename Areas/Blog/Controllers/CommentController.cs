using AspMVC.Models;
using AspMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace AspMVC.Areas.Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public CommentService _commentService;
        public CommentController(CommentService commentService, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _commentService = commentService;
        }


        [HttpGet]
        public JsonResult CreateComment()
        {
            //ClaimsPrincipal currentUser = this.User;
            //var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //Comment comment = new Comment()
            //{
            //    CommentContent = commentForm.Content,
            //    TimeStamp = DateTime.Now,
            //    //UserId = currentUserID,
            //    ProjectPageId = commentForm.PageId,
            //    //ParentCommentId = parentCommentId,
            //    IsDeleted = false
            //};
            

            //Task addComment = this._commentService.AddCommentAsync(comment);
            return new JsonResult(Ok());
        }

    }
}
