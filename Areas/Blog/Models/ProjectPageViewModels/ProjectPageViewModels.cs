using AspMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Collections;
using AspMVC.Models.EF;

namespace AspMVC.ViewModels
{
    public class ProjectPageDetailViewModel
    {
        public ProjectPageModel ProjectPage { get; set; }
        public CommentSectionViewModel CommentSection { get; set; }
    }
    public class DashboardViewModel
    {
        public IEnumerable<ProjectPageModel> ProjectPages { get;}
    }
    public class CreateProjectPageViewModel
    {
        public int ProjectID { get; set;}
        [Required(ErrorMessage = "Phải có tên Title")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Project Url")]
        public string? Slug { set; get; }

        [Display(Name = "Short Description")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        public string? ShortDescription { get; set; }


        [Display(Name = "Description")]
        public string? Description { get; set; }
        //Genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Name = "Platform")]
        public int PlatformId { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Chọn file upload")]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        //[Required(ErrorMessage = "Chọn một file")]
        public ICollection<IFormFile>? FileUpload { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "Chọn picture upload")]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        //[Required(ErrorMessage = "Chọn một file")]
        public ICollection<IFormFile>? PictureUpload { get; set; }
        public IFormFile? CoverPictureUpload { get; set; }

    }
    public class EditProjectPageViewModel : CreateProjectPageViewModel
    {

        public ProjectUploadedCoverImage? ProjectCover { get; set; }
        public RemovePictureCheckBox? RemoveCover { get; set; }
        public List<ProjectUploadedPicture>? ProjectGallery { get; set; }
        public List<RemovePictureCheckBox>? RemoveGallery { get;set; }
        public List<int>? RemoveFileIDs { get; set; }

    }
}
