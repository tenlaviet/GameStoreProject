using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class fixnullablePlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "ProjectPage",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage",
                column: "PlatformId",
                principalTable: "Platform",
                principalColumn: "PlatformId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "ProjectPage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPage_Platform_PlatformId",
                table: "ProjectPage",
                column: "PlatformId",
                principalTable: "Platform",
                principalColumn: "PlatformId");
        }
    }
}
