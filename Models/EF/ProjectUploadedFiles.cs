using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspMVC.Models.EF
{
    public class ProjectUploadedFile
    {
        [Key]
        public int FileID { get; set; }

        [Display(Name = "ProjectPage")]

        public int ProjectPageID { get; set; }
        [ForeignKey("ProjectPageID")]
        [Display(Name = "ProjectPage")]

        public ProjectPageModel ProjectPage { get; set; }

        public string ProjectFile { get; set; }
    }
}
