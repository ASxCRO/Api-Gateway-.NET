using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.API.Migrations
{
    public partial class oneToManyVoznjaUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoznjaKorisnik_Voznje_VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik");

            migrationBuilder.AlterColumn<int>(
                name: "VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VoznjaKorisnik_Voznje_VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik",
                column: "VoznjaId",
                principalSchema: "AutoStoper",
                principalTable: "Voznje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoznjaKorisnik_Voznje_VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik");

            migrationBuilder.AlterColumn<int>(
                name: "VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VoznjaKorisnik_Voznje_VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik",
                column: "VoznjaId",
                principalSchema: "AutoStoper",
                principalTable: "Voznje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
