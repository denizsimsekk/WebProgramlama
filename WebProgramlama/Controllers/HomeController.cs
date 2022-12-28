using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            viewModel.Kategoriler = _context.Kategoriler.ToList();
            return View(viewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> FotografEkle(Fotograf entity, IFormFile file,int page=1)
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
                _context.Fotograflar.Add(entity);
                _context.SaveChanges();
                return RedirectToAction("Kullanici", "Home", new { id = userId });
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
            viewModel.TumFotograflar = _context.Fotograflar.ToList();
            viewModel.secilenFotograf = _context.Fotograflar.ToList().Where(f=>f.FotografId==id).FirstOrDefault();
            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Kullanici(string id,int page=1)
        {
            viewModel.TumFotograflar = _context.Fotograflar.ToList();
            viewModel.secilenKullanici = _context.Kullanicilar.ToList().Where(k => k.Id == id).FirstOrDefault();
            viewModel.Fotograflar = _context.Fotograflar.ToList().Where(f=>f.KullaniciID==id).ToList();
            viewModel.Fotograflar = PaginatedResult(viewModel.Fotograflar, page, 10);
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
        public IActionResult Kategori(int id,int page=1)
        {
            ViewData["kategoriIsmi"] = _context.Kategoriler.ToList().Where(k => k.KategoriId == id).Select(k => k.KategoriIsmi).FirstOrDefault();
            var jobs = _context.Fotograflar.ToList().Where(f => f.KategoriID == id).ToList();
            return View(PaginatedResult(jobs,page,15));
        }

        public List<Fotograf> PaginatedResult(List<Fotograf> t, int page, int rowsPerPage)
        {
            @ViewBag.TotalRecords = t.Count;
            @ViewBag.CurrentPage = page;

            var skip = (page - 1) * rowsPerPage;

            var paginatedResult = t.Skip(skip).Take(rowsPerPage).ToList();
            return paginatedResult;
        }


        public IActionResult Fotograflar(int id, int page = 1)
        {
            
            var fotograflar= _context.Fotograflar.ToList(); 
            if (id == 0)
            {
                var jobs = _context.Fotograflar.ToList();


                fotograflar = PaginatedResult(jobs, page, 15);

            }
            else
            {
               
                var jobs = _context.Fotograflar.ToList().Where(f => f.KategoriID == id).ToList();
                fotograflar = PaginatedResult(jobs, page, 15);
            }
          
            return View(fotograflar);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}