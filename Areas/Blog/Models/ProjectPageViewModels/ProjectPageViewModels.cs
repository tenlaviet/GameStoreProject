using AspMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Collections;
using AspMVC.Models.EF;

namespace AspMVC.ViewModels
{
    public class CreatorPageViewModel
    {
        public List<ProjectPageModel> Projects { get; set; }
        public string CreatorName { get; set; }
    }
    public class ProjectPageDetailViewModel
    {
        public ProjectPageModel ProjectPage { get; set; }
        public CommentSectionViewModel CommentSection { get; set; }

        public RatingSectionViewModel? RatingSection { get; set; }
    }
    public class DashboardViewModel
    {
        public IEnumerable<ProjectPageModel> ProjectPages { get;}
    }
    public class CreateProjectPageViewModel
    {
        public int ProjectID { get; set;}
        [Required(ErrorMessage = "Project must have a title")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "must create url")]
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
        [Display(Name = "Upload Files")]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        //[Required(ErrorMessage = "Chọn một file")]
        public ICollection<IFormFile>? FileUpload { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Screenshots")]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        //[Required(ErrorMessage = "Chọn một file")]
        public ICollection<IFormFile>? PictureUpload { get; set; }
        [Display(Name = "Upload Cover Image")]
        public IFormFile? CoverPictureUpload { get; set; }

    }
    public class EditProjectPageViewModel : CreateProjectPageViewModel
    {
        public List<ProjectUploadedPicture>? ProjectGallery { get; set; }
        public ProjectUploadedCoverImage? ProjectCover { get; set; }
        [Display(Name = "Remove Cover Image")]
        public RemovePictureCheckBox? RemoveCover { get; set; }
        [Display(Name = "Remove Screenshots")]
        public List<RemovePictureCheckBox>? RemoveGallery { get;set; }
        [Display(Name = "Remove Files")]
        public List<int>? RemoveFileIDs { get; set; }

    }
}
