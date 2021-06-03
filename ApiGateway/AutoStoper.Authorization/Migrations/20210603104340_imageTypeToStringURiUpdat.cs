using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.Authorization.Migrations
{
    public partial class imageTypeToStringURiUpdat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUri",
                schema: "Auth",
                table: "Korisnici");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                schema: "Auth",
                table: "Korisnici",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Auth",
                table: "Korisnici");

            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                schema: "Auth",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
