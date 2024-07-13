using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Models.EF
{
    public class UserAvatar
    {
        [Key]
        public int AvatarId { get; set; }
        public string? Avatar { get; set;}
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        public string? AvatarRelativePath { get; set; }
    }
}
