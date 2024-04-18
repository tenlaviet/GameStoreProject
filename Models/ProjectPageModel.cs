using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Models
{
    [Table("ProjectPage")]

    public class ProjectPageModel
    {
        [Key]
        public int Id { get; set; }

        //// Category cha (FKey)
        //[Display(Name = "Danh mục cha")]
        //public int? ParentCategoryId { get; set; }

        // Tiều đề Category
        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; }

        // Nội dung, thông tin chi tiết về Category
        [DataType(DataType.Text)]
        [Display(Name = "Nội dung danh mục")]
        public string Description { set; get; }

        //chuỗi Url
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Project URL")]
        public string Slug { set; get; }
        [Display(Name = "Genre")]
        public int? GenreId { get; set; }
        [ForeignKey("GenreId")]
        [Display(Name = "Genre")]
        public Genre Genre { get; set; }
        
        // Các Category con
        //public ICollection<ProjectModel> ProjectPageChildren { get; set; }

        //[ForeignKey("ParentCategoryId")]
        //[Display(Name = "Danh mục cha")]


        //public ProjectModel ParentProjectPage { set; get; }
    }


}
