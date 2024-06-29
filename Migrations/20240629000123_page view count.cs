using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspMVC.Migrations
{
    public partial class pageviewcount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ProjectPage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ProjectPage");
        }
    }
}
