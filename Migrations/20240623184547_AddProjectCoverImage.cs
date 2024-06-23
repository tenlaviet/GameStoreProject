using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class AddProjectCoverImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCoverImage",
                table: "ProjectPage");

            migrationBuilder.AddColumn<string>(
                name: "ProjectPictureRelativePath",
                table: "ProjectUploadedPicture",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProjectUploadedCoverImage",
                columns: table => new
                {
                    CoverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectPageID = table.Column<int>(type: "int", nullable: false),
                    ProjectCoverImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectCoverImageRelativePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUploadedCoverImage", x => x.CoverID);
                    table.ForeignKey(
                        name: "FK_ProjectUploadedCoverImage_ProjectPage_ProjectPageID",
                        column: x => x.ProjectPageID,
                        principalTable: "ProjectPage",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUploadedCoverImage_ProjectPageID",
                table: "ProjectUploadedCoverImage",
                column: "ProjectPageID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUploadedCoverImage");

            migrationBuilder.DropColumn(
                name: "ProjectPictureRelativePath",
                table: "ProjectUploadedPicture");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCoverImage",
                table: "ProjectPage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
