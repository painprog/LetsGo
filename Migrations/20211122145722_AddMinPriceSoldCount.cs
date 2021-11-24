using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.Migrations
{
    public partial class AddMinPriceSoldCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MinPrice",
                table: "Events",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Events");
        }
    }
}
