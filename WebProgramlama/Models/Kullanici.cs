using Microsoft.AspNetCore.Identity;

namespace WebProgramlama.Models
{
    public class Kullanici : IdentityUser
    {
        public string KullaniciAd { get; set; }



        public string KullaniciSoyadi { get; set; }


        public ICollection<Fotograf> Fotograflar { get; set; }
    }
}
