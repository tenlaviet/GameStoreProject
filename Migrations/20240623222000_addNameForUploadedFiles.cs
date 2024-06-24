using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class addNameForUploadedFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "ProjectUploadedPicture",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ProjectUploadedFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverName",
                table: "ProjectUploadedCoverImage",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "ProjectUploadedPicture");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ProjectUploadedFile");

            migrationBuilder.DropColumn(
                name: "CoverName",
                table: "ProjectUploadedCoverImage");
        }
    }
}
