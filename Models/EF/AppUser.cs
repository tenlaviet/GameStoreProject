using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace AspMVC.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? HomeAdress { get; set; }

        // [Required]       
        [DataType(DataType.DateTime)]
        public DateTime? BirthDate { get; set; }
        public ICollection<Comment> Comments { get; } = new List<Comment>();
        public ICollection<ProjectPageModel> Projects { get; } = new List<ProjectPageModel>();
    }
}
