using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;
using AspMVC.Models.EF;

namespace AspMVC.Models
{
    [Table("ProjectPage")]
    public class ProjectPageModel
    {

        public ProjectPageModel() { }
        public ProjectPageModel(string creatorid, string title, string shdecription, string description, int genreid, string slug)
        {
            CreatorId = creatorid;
            Title = title;
            ShortDescription = shdecription;
            Description = description;
            GenreId = genreid;
            Slug = slug;
        }

        [Key]
        public int ProjectId { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public AppUser Creator { get; set; }

        // Tiều đề Category
        [Required(ErrorMessage = "Phải có tên Title")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        //chuỗi Url
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Project Url")]
        public string Slug { set; get; }
        // noi dung ngan' 
        [DataType(DataType.Text)]
        [Display(Name = "Short Description")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ShortDescription { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ProjectPageDatePosted { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //Genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        [Display(Name = "Genre")]
        public Genre Genre { set; get; }
        //Tag
        //Coverimage
        //VideoLink
        //Screenshots

        public string? ProjectImagesDir { get; set; }
        public string? ProjectFilesDir { get; set; }


        [Display(Name = "CoverImage")]
        public ProjectUploadedCoverImage? ProjectCoverImage { get; set; }

        public ICollection<ProjectUploadedFile> ProjectFiles { get; } = new List<ProjectUploadedFile>();

        public ICollection<ProjectUploadedPicture> ProjectPictures { get; } = new List<ProjectUploadedPicture>();

        public ICollection<Comment> Comments { get; } = new List<Comment>();

    }
}
