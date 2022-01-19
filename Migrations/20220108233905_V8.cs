using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Korisnik");

            migrationBuilder.AddColumn<int>(
                name: "KuvarID",
                table: "Sastojak",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sastojak_KuvarID",
                table: "Sastojak",
                column: "KuvarID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sastojak_Kuvar_KuvarID",
                table: "Sastojak",
                column: "KuvarID",
                principalTable: "Kuvar",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sastojak_Kuvar_KuvarID",
                table: "Sastojak");

            migrationBuilder.DropIndex(
                name: "IX_Sastojak_KuvarID",
                table: "Sastojak");

            migrationBuilder.DropColumn(
                name: "KuvarID",
                table: "Sastojak");

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "Korisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
