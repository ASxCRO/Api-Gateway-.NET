using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.API.Migrations
{
    public partial class adresaPropertiesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Distanca",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Vrijeme",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distanca",
                schema: "AutoStoper",
                table: "Adrese");

            migrationBuilder.DropColumn(
                name: "Lat",
                schema: "AutoStoper",
                table: "Adrese");

            migrationBuilder.DropColumn(
                name: "Lng",
                schema: "AutoStoper",
                table: "Adrese");

            migrationBuilder.DropColumn(
                name: "Vrijeme",
                schema: "AutoStoper",
                table: "Adrese");
        }
    }
}
