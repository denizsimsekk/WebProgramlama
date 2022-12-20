using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProgramlama.Data.Migrations
{
    public partial class baslangic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KullaniciAd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KullaniciSoyadi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriIsmi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.KategoriId);
                });

            migrationBuilder.CreateTable(
                name: "Fotograf",
                columns: table => new
                {
                    KullaniciID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KategoriID = table.Column<int>(type: "int", nullable: false),
                    FotografId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FotografAciklamasi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FotografURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    yuklenenFotograf = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotograf", x => x.FotografId);
                    table.ForeignKey(
                        name: "FK_Fotograf_AspNetUsers_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fotograf_Kategori_KategoriID",
                        column: x => x.KategoriID,
                        principalTable: "Kategori",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotograf_KategoriID",
                table: "Fotograf",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_Fotograf_KullaniciID",
                table: "Fotograf",
                column: "KullaniciID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotograf");

            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.DropColumn(
                name: "KullaniciAd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KullaniciSoyadi",
                table: "AspNetUsers");
        }
    }
}
