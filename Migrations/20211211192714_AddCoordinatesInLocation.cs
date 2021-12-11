using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.Migrations
{
    public partial class AddCoordinatesInLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "X",
                table: "Locations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Y",
                table: "Locations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Locations");
        }
    }
}
