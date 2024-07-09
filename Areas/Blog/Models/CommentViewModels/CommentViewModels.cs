using AspMVC.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AspMVC.ViewModels
{
    public class CommentSectionViewModel
    {
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public int PageId { get; set; }
        public bool isCreatorOrAdmin { get; set; }


    }
    public class CreateCommentViewModel
    {
        //public string UserId { get; set; }
        public int? PageId { get; set; }
        [Required(ErrorMessage = "Phải nhập {0}")]
        public string? Content { get; set; }

    }
    public class CommentCardViewModel
    {
        public int CommentID { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public string LastSent { get; set; }
        public bool isAuthor { get; set; }
        public bool isCreatorOrAdmin { get; set; }

    }
    public class RatingSectionViewModel
    {
        public int PageId { get; set; }
        public double RatingScore { get; set; }
        public string UserId { get; set;}
    }

}