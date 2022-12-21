using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProgramlama.Data.Migrations
{
    public partial class guncelleme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotograf_AspNetUsers_KullaniciID",
                table: "Fotograf");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotograf_Kategori_KategoriID",
                table: "Fotograf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategori",
                table: "Kategori");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotograf",
                table: "Fotograf");

            migrationBuilder.RenameTable(
                name: "Kategori",
                newName: "Kategoriler");

            migrationBuilder.RenameTable(
                name: "Fotograf",
                newName: "Fotograflar");

            migrationBuilder.RenameIndex(
                name: "IX_Fotograf_KullaniciID",
                table: "Fotograflar",
                newName: "IX_Fotograflar_KullaniciID");

            migrationBuilder.RenameIndex(
                name: "IX_Fotograf_KategoriID",
                table: "Fotograflar",
                newName: "IX_Fotograflar_KategoriID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategoriler",
                table: "Kategoriler",
                column: "KategoriId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotograflar",
                table: "Fotograflar",
                column: "FotografId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotograflar_AspNetUsers_KullaniciID",
                table: "Fotograflar",
                column: "KullaniciID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotograflar_Kategoriler_KategoriID",
                table: "Fotograflar",
                column: "KategoriID",
                principalTable: "Kategoriler",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotograflar_AspNetUsers_KullaniciID",
                table: "Fotograflar");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotograflar_Kategoriler_KategoriID",
                table: "Fotograflar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategoriler",
                table: "Kategoriler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotograflar",
                table: "Fotograflar");

            migrationBuilder.RenameTable(
                name: "Kategoriler",
                newName: "Kategori");

            migrationBuilder.RenameTable(
                name: "Fotograflar",
                newName: "Fotograf");

            migrationBuilder.RenameIndex(
                name: "IX_Fotograflar_KullaniciID",
                table: "Fotograf",
                newName: "IX_Fotograf_KullaniciID");

            migrationBuilder.RenameIndex(
                name: "IX_Fotograflar_KategoriID",
                table: "Fotograf",
                newName: "IX_Fotograf_KategoriID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategori",
                table: "Kategori",
                column: "KategoriId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotograf",
                table: "Fotograf",
                column: "FotografId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotograf_AspNetUsers_KullaniciID",
                table: "Fotograf",
                column: "KullaniciID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotograf_Kategori_KategoriID",
                table: "Fotograf",
                column: "KategoriID",
                principalTable: "Kategori",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
