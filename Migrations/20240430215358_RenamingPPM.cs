using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class RenamingPPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectFileUrl",
                table: "ProjectPage");

            migrationBuilder.AddColumn<string>(
                name: "ProjectFileDirectory",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectFileDirectory",
                table: "ProjectPage");

            migrationBuilder.AddColumn<string>(
                name: "ProjectFileUrl",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
