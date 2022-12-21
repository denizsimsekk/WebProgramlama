using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProgramlama.Models;

namespace WebProgramlama.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public DbSet<Fotograf> Fotograflar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(
                @"Server=localhost\SQLEXPRESS;Database=GaleriDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }
}