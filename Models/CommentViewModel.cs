using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AspMVC.Models
{
    public class CommentSectionViewModel
    {
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public int? PageId { get; set; }

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
        public int? CommentID { get; set; }
        public string? AuthorName { get; set; }
        public string? Content { get; set; }
        public string? LastSent { get; set; }
    }
    //public class ReplyCommentActionModel
    //{
    //    public int ID { get; set; }
    //    public int CommentID { get; set; }
    //    public string Text { get; set; }
    //    public int ProductID { get; set; }
    //}
}
