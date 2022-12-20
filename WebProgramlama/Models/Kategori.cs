using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlama.Models
{
    public class Kategori
    {
        [Key]
        [Column(Order = 1)]
        public int KategoriId { get; set; }

        [Required]
        [MaxLength(50)]
        public string KategoriIsmi { get; set; }
    }

}
