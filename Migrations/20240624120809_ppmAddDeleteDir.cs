using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class ppmAddDeleteDir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectFilesDir",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectImagesDir",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectFilesDir",
                table: "ProjectPage");

            migrationBuilder.DropColumn(
                name: "ProjectImagesDir",
                table: "ProjectPage");
        }
    }
}
