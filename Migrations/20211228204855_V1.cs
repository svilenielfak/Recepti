using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kuvar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kuvar", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sastojak",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojak", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Recept",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorisnikID = table.Column<int>(type: "int", nullable: true),
                    Ocena = table.Column<int>(type: "int", nullable: false),
                    BrojOcena = table.Column<int>(type: "int", nullable: false),
                    KuvarID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recept", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Recept_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recept_Kuvar_KuvarID",
                        column: x => x.KuvarID,
                        principalTable: "Kuvar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecKor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceptID = table.Column<int>(type: "int", nullable: true),
                    BrKorak = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecKor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecKor_Recept_ReceptID",
                        column: x => x.ReceptID,
                        principalTable: "Recept",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecOc",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceptID = table.Column<int>(type: "int", nullable: true),
                    KorisnikID = table.Column<int>(type: "int", nullable: true),
                    Ocena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecOc", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecOc_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecOc_Recept_ReceptID",
                        column: x => x.ReceptID,
                        principalTable: "Recept",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecSas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceptID = table.Column<int>(type: "int", nullable: true),
                    SastojakID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecSas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecSas_Recept_ReceptID",
                        column: x => x.ReceptID,
                        principalTable: "Recept",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecSas_Sastojak_SastojakID",
                        column: x => x.SastojakID,
                        principalTable: "Sastojak",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recept_KorisnikID",
                table: "Recept",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Recept_KuvarID",
                table: "Recept",
                column: "KuvarID");

            migrationBuilder.CreateIndex(
                name: "IX_RecKor_ReceptID",
                table: "RecKor",
                column: "ReceptID");

            migrationBuilder.CreateIndex(
                name: "IX_RecOc_KorisnikID",
                table: "RecOc",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_RecOc_ReceptID",
                table: "RecOc",
                column: "ReceptID");

            migrationBuilder.CreateIndex(
                name: "IX_RecSas_ReceptID",
                table: "RecSas",
                column: "ReceptID");

            migrationBuilder.CreateIndex(
                name: "IX_RecSas_SastojakID",
                table: "RecSas",
                column: "SastojakID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecKor");

            migrationBuilder.DropTable(
                name: "RecOc");

            migrationBuilder.DropTable(
                name: "RecSas");

            migrationBuilder.DropTable(
                name: "Recept");

            migrationBuilder.DropTable(
                name: "Sastojak");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Kuvar");
        }
    }
}
