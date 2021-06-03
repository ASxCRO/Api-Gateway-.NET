using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.API.Migrations
{
    public partial class adresaPropertiesUpdateForOdrediste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lng",
                schema: "AutoStoper",
                table: "Adrese",
                newName: "LngPolaziste");

            migrationBuilder.RenameColumn(
                name: "Lat",
                schema: "AutoStoper",
                table: "Adrese",
                newName: "LngOdrediste");

            migrationBuilder.AddColumn<double>(
                name: "LatOdrediste",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LatPolaziste",
                schema: "AutoStoper",
                table: "Adrese",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatOdrediste",
                schema: "AutoStoper",
                table: "Adrese");

            migrationBuilder.DropColumn(
                name: "LatPolaziste",
                schema: "AutoStoper",
                table: "Adrese");

            migrationBuilder.RenameColumn(
                name: "LngPolaziste",
                schema: "AutoStoper",
                table: "Adrese",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "LngOdrediste",
                schema: "AutoStoper",
                table: "Adrese",
                newName: "Lat");
        }
    }
}
