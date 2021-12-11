using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.Migrations
{
    public partial class AddCoordinatesOnModelCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "X", "Y" },
                values: new object[] { 42.817987000000002, 74.620718999999994 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "X", "Y" },
                values: new object[] { 42.855733999999998, 74.587316000000001 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "X", "Y" },
                values: new object[] { 42.880873000000001, 74.596663000000007 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "X", "Y" },
                values: new object[] { 42.878090999999998, 74.612414999999999 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "X", "Y" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "X", "Y" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "X", "Y" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "X", "Y" },
                values: new object[] { 0.0, 0.0 });
        }
    }
}
