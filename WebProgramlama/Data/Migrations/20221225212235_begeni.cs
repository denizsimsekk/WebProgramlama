using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProgramlama.Data.Migrations
{
    public partial class begeni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "begeniSayisi",
                table: "Fotograflar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "begeniSayisi",
                table: "Fotograflar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
