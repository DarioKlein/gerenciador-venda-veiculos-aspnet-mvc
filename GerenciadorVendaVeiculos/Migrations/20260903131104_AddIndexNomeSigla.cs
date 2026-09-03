using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorVendaVeiculos.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexNomeSigla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Marca_Nome",
                table: "Marca",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marca_Sigla",
                table: "Marca",
                column: "Sigla",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Marca_Nome",
                table: "Marca");

            migrationBuilder.DropIndex(
                name: "IX_Marca_Sigla",
                table: "Marca");
        }
    }
}
