using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Models.EF
{
    public class ProjectUploadedCoverImage
    {
        [Key]
        public int CoverID { get; set; }
        public string? CoverName { get; set; }

        [Display(Name = "ProjectPage")]
        public int ProjectPageID { get; set; }

        [ForeignKey("ProjectPageID")]
        [Display(Name = "ProjectPage")]
        public ProjectPageModel ProjectPage { get; set; }

        public string ProjectCoverImage { get; set; }
        public string ProjectCoverImageRelativePath { get; set; }
    }
}
