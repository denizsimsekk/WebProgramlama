using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<Kullanici> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;


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

            return View();
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
                entity.KategoriID = 1;
                //entity.DateAdded = DateTime.Now;
                _context.Fotograflar.Add(entity);
                _context.SaveChanges();
                return RedirectToAction("FotografListele");
            }

            return View();
        }

        public IActionResult FotografListele()
        {
            var fotograflar = _context.Fotograflar.ToList();
            return View(fotograflar);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}