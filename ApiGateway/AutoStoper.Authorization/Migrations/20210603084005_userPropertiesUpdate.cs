using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.Authorization.Migrations
{
    public partial class userPropertiesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Auth",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "UserImage",
                schema: "Auth",
                table: "Korisnici",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Auth",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "UserImage",
                schema: "Auth",
                table: "Korisnici");
        }
    }
}
