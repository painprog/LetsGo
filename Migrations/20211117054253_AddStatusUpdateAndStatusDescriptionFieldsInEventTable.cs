using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsGo.Migrations
{
    public partial class AddStatusUpdateAndStatusDescriptionFieldsInEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusDescription",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdate",
                table: "Events",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDescription",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StatusUpdate",
                table: "Events");
        }
    }
}
