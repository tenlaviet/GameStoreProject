using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspMVC.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string CommentContent { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public AppUser Author { get; set; }
        public int? ProjectPageId { get; set; }
        [ForeignKey("ProjectPageId")]
        public ProjectPageModel? ProjectPage { get; set; }

        public int? ParentCommentId { get; set; }
        [ForeignKey("ParentCommentId")]
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> CommentChildren { get; set; }


        public bool IsDeleted { get; set; }



    }
}
