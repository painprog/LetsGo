using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.DAL.Migrations
{
    public partial class AddSelfInfoFieldInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelfInfo",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelfInfo",
                table: "AspNetUsers");
        }
    }
}
