using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AspMVC.Models
{
    public class CreateGenreVM
    {
        public int GenreId { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [Required(ErrorMessage = "Phải nhập  {0}")]
        [Display(Name = "Genre")]
        public string GenreName { get; set; }

    }
}
