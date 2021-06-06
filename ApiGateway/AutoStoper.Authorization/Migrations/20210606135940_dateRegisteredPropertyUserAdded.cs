using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.Authorization.Migrations
{
    public partial class dateRegisteredPropertyUserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistered",
                schema: "Auth",
                table: "Korisnici",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRegistered",
                schema: "Auth",
                table: "Korisnici");
        }
    }
}
