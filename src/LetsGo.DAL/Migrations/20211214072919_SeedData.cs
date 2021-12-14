using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.DAL.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventCategories",
                columns: new[] { "Id", "HasParent", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, false, "Концерты", null },
                    { 16, true, "Вокал", 5 },
                    { 15, true, "Балет", 5 },
                    { 13, true, "Мелодрамы", 3 },
                    { 12, true, "Драмы", 3 },
                    { 11, true, "Комедии", 3 },
                    { 10, true, "Хип-Хоп", 1 },
                    { 9, true, "Рок", 1 },
                    { 14, true, "Опера", 5 },
                    { 7, false, "Другое", null },
                    { 6, false, "Экскурсии", null },
                    { 5, false, "Классика", null },
                    { 4, false, "Детям", null },
                    { 3, false, "Спектакли", null },
                    { 2, false, "Фестивали", null },
                    { 8, true, "Поп-Музыка", 1 }
                });

            migrationBuilder.InsertData(
                table: "LocationCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Классика" },
                    { 4, "Детям" },
                    { 3, "Спектакли" },
                    { 2, "Фестивали" },
                    { 7, "Другое" },
                    { 6, "Экскурсии" },
                    { 1, "Концерты" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Categories", "Description", "LocationImage", "Name", "Phones", "X", "Y" },
                values: new object[,]
                {
                    { 4, "167 Советская, Бишкек", "[{\"Id\":\"1\",\"Name\":\"Театры\"}]", "Театр оперы и балета", null, "Театр Оперы и Балета", "[\"0312 621 619\"]", 42.878090999999998, 74.612414999999999 },
                    { 3, "17 ул. Тоголок Молдо, Бишкек", "[{\"Id\":\"5\",\"Name\":\"Cпортивные комплексы\"}, {\"Id\":\"10\",\"Name\":\"Другое\"}]", "Концертный зал", null, "Стадион Спартак", "[\"+996705 000 888\"]", 42.880873000000001, 74.596663000000007 },
                    { 1, "21, 11 Аалы Токомбаева көчөсү, Бишкек", "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]", "Художественный центр в Бишкеке", null, "Асанбай Центр", "[\"+996775979500\"]", 42.817987000000002, 74.620718999999994 },
                    { 2, "24 просп. Мира, Бишкек", "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]", "Концертный зал", null, "Ретро-Метро", "[\"+996705 000 888\"]", 42.855733999999998, 74.587316000000001 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LocationCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
