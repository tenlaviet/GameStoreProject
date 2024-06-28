using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class GenreSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Genres",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "action");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Genres");
        }
    }
}
