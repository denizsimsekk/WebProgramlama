﻿using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using WebProgramlama.Models;
using WebProgramlama.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgramlama.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Kullanici> _userManager;
        private FotografKullaniciViewModel viewModel = new FotografKullaniciViewModel();
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


        public List<Kullanici> PaginatedResult(List<Kullanici> t, int page, int rowsPerPage)
        {
            @ViewBag.TotalRecords = t.Count;
            @ViewBag.CurrentPage = page;

            var skip = (page - 1) * rowsPerPage;

            var paginatedResult = t.Skip(skip).Take(rowsPerPage).ToList();
            return paginatedResult;
        }


        public IActionResult KullaniciListele(int pg=1,int page=1)
        {

            var jobs = _context.Kullanicilar
            .ToList();

            viewModel.Kullanicilar = PaginatedResult(jobs, page, 10);
        
            viewModel.Fotograflar = _context.Fotograflar.ToList();
     
            
            return View(viewModel);
        }



        public IActionResult KullaniciEkleSayfasi()
        {

            return View();
        }

        

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
        public IActionResult KategoriSil(int kategoriID)
        {
            var kategori = _context.Kategoriler.Find(kategoriID);
            _context.Kategoriler.Remove(kategori);
            _context.SaveChanges();

            return RedirectToAction("KategoriListele", "Admin");
        }
        public IActionResult FotografSil(int fotogradId)
        {

            var fotograf = _context.Fotograflar.Find(fotogradId);
            _context.Fotograflar.Remove(fotograf);
            _context.SaveChanges();
            return RedirectToAction("PagedFotograflar", "Admin");
        }


        public List<Fotograf> PaginatedResult(List<Fotograf> t, int page, int rowsPerPage)
        {
            @ViewBag.TotalRecords = t.Count;
            @ViewBag.CurrentPage = page;

            var skip = (page - 1) * rowsPerPage;

            var paginatedResult = t.Skip(skip).Take(rowsPerPage).ToList();
            return paginatedResult;
        }

        public IActionResult PagedFotograflar(string search = null, int page = 1)
        {
            var jobs = _context.Fotograflar
                       .Where(x => x.FotografAciklamasi.Contains(search) || search == null)
                       .ToList();

            var paginatedResult = PaginatedResult(jobs, page, 10);
            viewModel.Fotograflar = PaginatedResult(jobs, page, 10);
            viewModel.Kullanicilar = _context.Kullanicilar.ToList();
            return View(viewModel);
        }
       
        public IActionResult FotografEkleSayfasi()
        {
            viewModel.Fotograflar = _context.Fotograflar.ToList();
            viewModel.Kategoriler = _context.Kategoriler.ToList();

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> FotografEkle(Fotograf entity, IFormFile file)
        {

            string userId;
            var user = User.Identity.Name;
            userId = _context.Kullanicilar.Where(x => x.Email == user).Select(y => y.Id).ToString();
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
                return RedirectToAction("FotografListele");
            }

            return View();
        }

    }
}

