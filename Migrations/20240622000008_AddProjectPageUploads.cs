using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class AddProjectPageUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectFileDirectory",
                table: "ProjectPage");

            migrationBuilder.CreateTable(
                name: "ProjectUploadedFile",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectPageID = table.Column<int>(type: "int", nullable: false),
                    ProjectFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUploadedFile", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_ProjectUploadedFile_ProjectPage_ProjectPageID",
                        column: x => x.ProjectPageID,
                        principalTable: "ProjectPage",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUploadedPicture",
                columns: table => new
                {
                    PictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectPageID = table.Column<int>(type: "int", nullable: false),
                    ProjectPictureFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUploadedPicture", x => x.PictureID);
                    table.ForeignKey(
                        name: "FK_ProjectUploadedPicture_ProjectPage_ProjectPageID",
                        column: x => x.ProjectPageID,
                        principalTable: "ProjectPage",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUploadedFile_ProjectPageID",
                table: "ProjectUploadedFile",
                column: "ProjectPageID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUploadedPicture_ProjectPageID",
                table: "ProjectUploadedPicture",
                column: "ProjectPageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUploadedFile");

            migrationBuilder.DropTable(
                name: "ProjectUploadedPicture");

            migrationBuilder.AddColumn<string>(
                name: "ProjectFileDirectory",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
