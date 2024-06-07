using AspMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Areas.Blog.Models.Project
{
    public class ProjectPageDetailViewModel
    {
        ////for download path
        //public int? ProjectID { get; set; }
        //public string ProjectTitle { get; set; }
        //public string PageSlug { set; get; }
        //public string ProjectShortDescription { get; set; }
        //public string ProjectDescription { get; set; }
        //public string GenreName { set; get; }
        //public string ProjectFileDirectory { get; set; }
        public ProjectPageModel ProjectPage { get; set; }
        public CommentSectionViewModel CommentSection { get; set; }
    }
    public class CreateProjectPageViewModel
    {

        [Required(ErrorMessage = "Phải có tên Title")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Project Url")]
        public string Slug { set; get; }

        [Display(Name = "Short Description")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ShortDescription { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }
        //Genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        //[Required(ErrorMessage = "Chọn một file")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Chọn file upload")]
        public IFormFile FileUpload { get; set; }

    }
}
