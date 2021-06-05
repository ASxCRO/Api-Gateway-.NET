using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.API.Migrations
{
    public partial class dodanPropertyCijenaNaVoznju : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cijena",
                schema: "AutoStoper",
                table: "Voznje",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cijena",
                schema: "AutoStoper",
                table: "Voznje");
        }
    }
}
