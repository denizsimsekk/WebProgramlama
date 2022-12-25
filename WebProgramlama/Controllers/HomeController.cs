using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebProgramlama.Data;
using WebProgramlama.Models;

namespace WebProgramlama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Kullanici> _userManager;
        string userId;
        private FotografKullaniciViewModel viewModel = new FotografKullaniciViewModel();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<Kullanici> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;


        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
            );

            return LocalRedirect(returnUrl);
        }
        
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Authorize]
        public IActionResult FotografEkleSayfasi()
        {
            //viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();

            return View(viewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> FotografEkle(Fotograf entity, IFormFile file)
        {
            var user = User.Identity.Name;
            userId = _context.Kullanicilar.Where(x => x.Email == user).Select(y => y.Id).FirstOrDefault();
            

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
                entity.KullaniciID = userId;
                // fotograf.Kullanici = (Kullanici)(from k in _context.Kullanicilar where k.KullaniciID == 1 select k);
                
                //entity.DateAdded = DateTime.Now;
                _context.Fotograflar.Add(entity);
                _context.SaveChanges();
                return RedirectToAction("Kategori",entity.KategoriID);
            }

            return View();
        }

        public IActionResult FotografListele()
        {
            var fotograflar = _context.Fotograflar.ToList();
            return View(fotograflar);
        }


        public IActionResult Fotograf(int id)
        {

            viewModel.secilenFotograf = _context.Fotograflar.ToList().Where(f=>f.FotografId==id).FirstOrDefault();
            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();


            return View(viewModel);
        }
        public IActionResult Kullanici(string id)
        {

            viewModel.secilenKullanici = _context.Kullanicilar.ToList().Where(k => k.Id == id).FirstOrDefault();
            viewModel.Fotograflar = _context.Fotograflar.ToList().Where(f=>f.KullaniciID==id).ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();


            return View(viewModel);
        }
        public IActionResult KategoriListele()
        {

            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();


            return View(viewModel);
        }
        public IActionResult Kategori(int id)
        {

            //viewModel.secilenKullanici = _context.Kullanicilar.ToList().Where(k => k.Id == id).FirstOrDefault();
            viewModel.Fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID == id).ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList().Where(f => f.KategoriId == id).ToList();
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();


            return View(viewModel);
        }

        public IActionResult Fotograflar(int id)
        {
            var fotograflar= _context.Fotograflar.ToList(); 
            if (id == 0)
            {
                 fotograflar = _context.Fotograflar.ToList();

            }
            else if (id == 1)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f=>f.KategoriID==1).ToList();

            }
            else if (id == 2)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID == 2).ToList();

            }
            else if (id == 3)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID == 3).ToList();

            }
            else if (id == 4)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID == 4).ToList();

            }
            else if (id == 5)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID ==5).ToList();

            }
            else if (id == 7)
            {
                //linq
                fotograflar = _context.Fotograflar.ToList().Where(f => f.KategoriID == 4).ToList();

            }
            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();

            return View(fotograflar);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}