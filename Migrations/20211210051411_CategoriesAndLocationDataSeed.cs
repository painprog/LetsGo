using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.Migrations
{
    public partial class CategoriesAndLocationDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasParent",
                table: "EventCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "EventCategories",
                columns: new[] { "Id", "HasParent", "Name", "ParentId" },
                values: new object[,]
                {
                    { "1", false, "Концерты", null },
                    { "17", true, "Вокал", "5" },
                    { "16", true, "Балет", "5" },
                    { "15", true, "Опера", "5" },
                    { "13", true, "Драмы", "3" },
                    { "12", true, "Комедии", "3" },
                    { "11", true, "Хип-Хоп", "1" },
                    { "10", true, "Рок", "1" },
                    { "14", true, "Мелодрамы", "3" },
                    { "8", false, "Другое", null },
                    { "7", false, "Экскурсии", null },
                    { "6", false, "Экскурсии", null },
                    { "5", false, "Классика", null },
                    { "4", false, "Детям", null },
                    { "3", false, "Спектакли", null },
                    { "2", false, "Фестивали", null },
                    { "9", true, "Поп-Музыка", "1" }
                });

            migrationBuilder.InsertData(
                table: "LocationCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "5", "Классика" },
                    { "4", "Детям" },
                    { "3", "Спектакли" },
                    { "2", "Фестивали" },
                    { "7", "Другое" },
                    { "6", "Экскурсии" },
                    { "1", "Концерты" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Categories", "Description", "LocationImage", "Name", "Phones" },
                values: new object[,]
                {
                    { "4", "167 Советская, Бишкек", "[{\"Id\":\"1\",\"Name\":\"Театры\"}]", "Театр оперы и балета", null, "Театр Оперы и Балета", "[\"0312 621 619\"]" },
                    { "3", "17 ул. Тоголок Молдо, Бишкек", "[{\"Id\":\"5\",\"Name\":\"Cпортивные комплексы\"}, {\"Id\":\"10\",\"Name\":\"Другое\"}]", "Концертный зал", null, "Стадион Спартак", "[\"+996705 000 888\"]" },
                    { "1", "21, 11 Аалы Токомбаева көчөсү, Бишкек", "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]", "Художественный центр в Бишкеке", null, "Асанбай Центр", "[\"+996775979500\"]" },
                    { "2", "24 просп. Мира, Бишкек", "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]", "Концертный зал", null, "Ретро-Метро", "[\"+996705 000 888\"]" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "17");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DropColumn(
                name: "HasParent",
                table: "EventCategories");
        }
    }
}
