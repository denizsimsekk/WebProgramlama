using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using WebProgramlama.Models;
using WebProgramlama.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace WebProgramlama.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        //private readonly DbContext _context;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Kullanici> _userManager;
        private FotografKullaniciViewModel viewModel = new FotografKullaniciViewModel();


        string userId;

        
        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, UserManager<Kullanici> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
           

        }

     
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult KullaniciListele()
        {
            var kullanicilar = _context.Kullanicilar.ToList();
            return View(kullanicilar);
        }



        public IActionResult KullaniciEkleSayfasi()
        {

            return View();
        }

        
        //[HttpPost]
        //public IActionResult KullaniciEkle(Kullanici kullanici)
        //{
        //    string p = kullanici.PasswordHash;
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        //    Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

        //    // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: p!,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 100000,
        //        numBytesRequested: 256 / 8));
        //    kullanici.PasswordHash = hashed;

        //    _context.Kullanicilar.Add(kullanici);
        //    _context.SaveChanges();

        //    return RedirectToAction("KullaniciListele", "Admin");
        //}

        public IActionResult KategoriEkleSayfasi()
        {

            return View();
        }



        [HttpPost]
        public IActionResult KategoriEkle(Kategori kategori)
        {
            _context.Kategoriler.Add(kategori);
            _context.SaveChanges();

            return RedirectToAction("KategoriListele", "Admin");
        }

        public IActionResult KategoriListele()
        {
            var kategoriler = _context.Kategoriler.ToList();
            return View(kategoriler);
        }

        public IActionResult KullaniciDuzenle(string kullaniciID)
        {
            var kullanici = _context.Kullanicilar.Find(kullaniciID);
            // _context.Kullanicilar.Remove(kullanici);

            return View(kullanici);
        }
        [HttpPost]
        public IActionResult KullaniciUpdate(Kullanici yeniKullanici, string kullaniciID)
        {
            var kullanici = _context.Kullanicilar.Find(kullaniciID);
            kullanici.KullaniciAd = yeniKullanici.KullaniciAd;
            kullanici.KullaniciSoyadi = yeniKullanici.KullaniciSoyadi;
            _context.Kullanicilar.Update(kullanici);
            _context.SaveChanges();

            return RedirectToAction("KullaniciListele", "Admin");
        }

        public IActionResult KullaniciSil(int kullaniciID)
        {
            var kullanici = _context.Kullanicilar.Find(kullaniciID);
            _context.Kullanicilar.Remove(kullanici);
            _context.SaveChanges();

            return RedirectToAction("KullaniciListele", "Admin");
        }

        public IActionResult FotografSil(int fotogradId)
        {

            var fotograf = _context.Fotograflar.Find(fotogradId);
            _context.Fotograflar.Remove(fotograf);
            _context.SaveChanges();
            return RedirectToAction("FotografListele", "Admin");
        }

        public IActionResult FotografListele()
        {

          
        viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();
            return View(viewModel);
        }

        public IActionResult FotografEkleSayfasi()
        {
            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();

            return View(viewModel);
        }


        /*[HttpPost]
        public IActionResult FotografEkle(Fotograf fotograf,HttpPos file)
        {

            if (Request.Form.Files.Count > 0 )
            {
                string dosyaAdi = Path.GetFileName(Request.Form.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Form.Files[0].FileName);
                //string dbAd = hayvan.Adi + hayvan.CinsId.ToString() + hayvan.TurId.ToString() + hayvan.Yasi.ToString() + hayvan.EkBilgiler + uzanti;
                fotograf.KullaniciID = 1;
               // fotograf.Kullanici = (Kullanici)(from k in _context.Kullanicilar where k.KullaniciID == 1 select k);
                fotograf.KategoriID = 1;
                //fotograf.Kategori = (Kategori)(from k in _context.Kategoriler where k.KategoriId == 1 select k);
                string fotoAd = fotograf.KullaniciID.ToString() + fotograf.KategoriID.ToString()+uzanti;
                string yol = "wwwroot/img/" + fotoAd; //+ uzanti;
                using (FileStream fs = System.IO.File.Create(yol))
                {
                    Request.Form.Files[0].CopyTo(fs);
                    fs.Flush();
                }
                fotograf.FotografURL = "/img/" + fotoAd;
                //hayvan.Cins = _context.Cins.Find(hayvan.CinsId);
                //hayvan.Tur = _context.Tur.Find(hayvan.TurId);
                _context.Fotograflar.Add(fotograf);
                _context.SaveChanges();
                ViewBag.Mesaj = "Ekleme Başarılı";
                return RedirectToAction("FotografListele", "Admin");
            }
            ViewBag.Error = "Ekleme başarısız!";
            return View();

        }*
        [HttpPost]
        public async Task<IActionResult> FotografEkle(Fotograf fotograf, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", file.FileName);
                    var path_tn = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products\\tn", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        fotograf.yuklenenFotograf = file.FileName;
                    }

                    using (var stream = new FileStream(path_tn, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                    }
                }
                entity.DateAdded = DateTime.Now;
                unitOfWork.Products.Add(entity);
                unitOfWork.SaveChanges();
                return RedirectToAction("CatalogList");
            }

            return View(entity);
            if(file.Length>0)
            {
                using (var stream=new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    fotograf.yuklenenFotograf = stream.ToArray();
                }
                fotograf.KullaniciID = 1;
                // fotograf.Kullanici = (Kullanici)(from k in _context.Kullanicilar where k.KullaniciID == 1 select k);
                fotograf.KategoriID = 1;
                _context.Fotograflar.Add(fotograf);
                _context.SaveChanges();
                return RedirectToAction("FotografListele", "Admin");
            }
            return View();
        }*/

        [HttpPost]
        public async Task<IActionResult> FotografEkle(Fotograf entity, IFormFile file)
        {
            var user = User.Identity.Name;
            userId = _context.Kullanicilar.Where(x => x.Email == user).Select(y => y.Id).ToString();
            Console.WriteLine(entity.KategoriID.ToString()+" kategorisi");
            if (file != null)
            {

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\", file.FileName);
                var path_tn = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    entity.FotografURL = file.FileName;
                }
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    entity.yuklenenFotograf = stream.ToArray();
                }
                using (var stream = new FileStream(path_tn, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                entity.KullaniciID = "ab599adb-5a0f-4eec-b132-71b08c0cb69f";
                // fotograf.Kullanici = (Kullanici)(from k in _context.Kullanicilar where k.KullaniciID == 1 select k);
                
                //entity.DateAdded = DateTime.Now;
                _context.Fotograflar.Add(entity);
                _context.SaveChanges();
                return RedirectToAction("FotografListele");
            }

            return View();
        }

        //public FileContentResult getImage(Fotograf fotograf)
        //{
        //    //var fotograflar = _context.Fotograflar.ToList();
        //    //var fotograf= (Fotograf)(from f in _context.Fotograflar where f.KullaniciID == 1 select f);
        //    var bytes = fotograf.yuklenenFotograf.ToArray();
        //    var bytesMemoryStream = new MemoryStream(bytes);
        //    Image img = Image.FromStream(bytesMemoryStream);
        //    return new FileContentResult(bytes, "image/jpg");
        //}

    }
}

