using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class AddPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Genres",
                newName: "GenreSlug");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "GenreId");

            migrationBuilder.AddColumn<int>(
                name: "PlatformId",
                table: "ProjectPage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    PlatformId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlatformSlug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.PlatformId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPage_PlatformId",
                table: "ProjectPage",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage",
                column: "PlatformId",
                principalTable: "Platform",
                principalColumn: "PlatformId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPage_PlatformId",
                table: "ProjectPage");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "ProjectPage");

            migrationBuilder.RenameColumn(
                name: "GenreSlug",
                table: "Genres",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Genres",
                newName: "Id");
        }
    }
}
