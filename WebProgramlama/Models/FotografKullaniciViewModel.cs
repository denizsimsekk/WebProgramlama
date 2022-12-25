namespace WebProgramlama.Models
{
    public class FotografKullaniciViewModel
    {
        public Fotograf secilenFotograf { get; set; }
        public Kullanici secilenKullanici { get; set; }
        public List<Fotograf> Fotograflar { get; set; }
        public List<Kullanici> Kullanicilar { get; set; }
        public List<Kategori> Kategoriler { get; set; }
    }
}
