using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AspMVC.Models.EF
{
    public class ProjectRating
    {
        [Key]
        public int RateId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        public int ProjectPageId { get; set; }
        [ForeignKey("ProjectPageId")]
        public ProjectPageModel ProjectPage { get; set; }

        public double RatingScore { get; set; }
    }
}
