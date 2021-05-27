using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoStoper.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AutoStoper");

            migrationBuilder.CreateTable(
                name: "Adrese",
                schema: "AutoStoper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Polaziste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odrediste = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adrese", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voznje",
                schema: "AutoStoper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LjubimciDozvoljeni = table.Column<bool>(type: "bit", nullable: false),
                    PusenjeDozvoljeno = table.Column<bool>(type: "bit", nullable: false),
                    AutomatskoOdobrenje = table.Column<bool>(type: "bit", nullable: false),
                    MaksimalnoPutnika = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdresaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voznje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voznje_Adrese_AdresaId",
                        column: x => x.AdresaId,
                        principalSchema: "AutoStoper",
                        principalTable: "Adrese",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoznjaKorisnik",
                schema: "AutoStoper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Vozac = table.Column<bool>(type: "bit", nullable: false),
                    VoznjaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoznjaKorisnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoznjaKorisnik_Voznje_VoznjaId",
                        column: x => x.VoznjaId,
                        principalSchema: "AutoStoper",
                        principalTable: "Voznje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoznjaKorisnik_VoznjaId",
                schema: "AutoStoper",
                table: "VoznjaKorisnik",
                column: "VoznjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Voznje_AdresaId",
                schema: "AutoStoper",
                table: "Voznje",
                column: "AdresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoznjaKorisnik",
                schema: "AutoStoper");

            migrationBuilder.DropTable(
                name: "Voznje",
                schema: "AutoStoper");

            migrationBuilder.DropTable(
                name: "Adrese",
                schema: "AutoStoper");
        }
    }
}
