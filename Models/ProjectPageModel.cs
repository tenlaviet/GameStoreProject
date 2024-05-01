using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Models
{
    [Table("ProjectPage")]
    public class ProjectPageModel
    {

        public ProjectPageModel() { }
        public ProjectPageModel(string title, string shdecription, string description, int genreid, string slug, string fileDirectory) {   
            
               Title = title;
               ShortDescription = shdecription;
               Description = description;
               GenreId = genreid;
               Slug = slug;
               ProjectFileDirectory = fileDirectory;
        }

        [Key]
        public int Id { get; set; }

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

        //public ProjectClass Classification { get; set; }
        //public ProjectStatus ReleaseStatus { get; set; }
        //[Pricing]
        //[Upload]


        /// <summary>
        /// Details
        /// </summary>
        // Nội dung, thông tin chi tiết về Category
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

        public string ProjectFileDirectory { get; set; }
    }
}
