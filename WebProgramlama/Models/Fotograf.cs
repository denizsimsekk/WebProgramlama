using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebProgramlama.Models
{
    public class Fotograf
    {
        [Key]
        public int FotografId { get; set; }
        
        [MaxLength(200)]

        public string FotografAciklamasi { get; set; }

        [ForeignKey("Kullanici")]
        [Column(Order = 1)]
        public string KullaniciID { get; set; }
        public Kullanici Kullanici { get; set; }

        [ForeignKey("Kategori")]
        [Column(Order = 2)]
        public int KategoriID { get; set; }

        public Kategori Kategori { get; set; }

        [Required]
        public string FotografURL { get; set; }

        [Required]
        public byte[] yuklenenFotograf { get; set; }

       


    }
}
