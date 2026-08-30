using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorVendaVeiculos.Migrations
{
    /// <inheritdoc />
    public partial class AddRelacionamentoClienteCidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CidadeId",
                table: "Cliente",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CidadeId",
                table: "Cliente",
                column: "CidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente",
                column: "CidadeId",
                principalTable: "Cidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_CidadeId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "CidadeId",
                table: "Cliente");
        }
    }
}
