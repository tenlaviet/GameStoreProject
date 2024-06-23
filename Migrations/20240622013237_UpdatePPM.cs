using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class UpdatePPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectPictureFilePath",
                table: "ProjectUploadedPicture",
                newName: "ProjectPicture");

            migrationBuilder.RenameColumn(
                name: "ProjectFilePath",
                table: "ProjectUploadedFile",
                newName: "ProjectFile");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCoverImage",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectPageDatePosted",
                table: "ProjectPage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCoverImage",
                table: "ProjectPage");

            migrationBuilder.DropColumn(
                name: "ProjectPageDatePosted",
                table: "ProjectPage");

            migrationBuilder.RenameColumn(
                name: "ProjectPicture",
                table: "ProjectUploadedPicture",
                newName: "ProjectPictureFilePath");

            migrationBuilder.RenameColumn(
                name: "ProjectFile",
                table: "ProjectUploadedFile",
                newName: "ProjectFilePath");
        }
    }
}
