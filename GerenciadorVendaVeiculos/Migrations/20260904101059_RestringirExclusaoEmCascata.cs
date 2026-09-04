using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorVendaVeiculos.Migrations
{
    /// <inheritdoc />
    public partial class RestringirExclusaoEmCascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_Marca_MarcaId",
                table: "Veiculo");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Veiculo_VeiculoId",
                table: "Venda");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente",
                column: "CidadeId",
                principalTable: "Cidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_Marca_MarcaId",
                table: "Veiculo",
                column: "MarcaId",
                principalTable: "Marca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Veiculo_VeiculoId",
                table: "Venda",
                column: "VeiculoId",
                principalTable: "Veiculo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_Marca_MarcaId",
                table: "Veiculo");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Veiculo_VeiculoId",
                table: "Venda");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Cidade_CidadeId",
                table: "Cliente",
                column: "CidadeId",
                principalTable: "Cidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_Marca_MarcaId",
                table: "Veiculo",
                column: "MarcaId",
                principalTable: "Marca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Veiculo_VeiculoId",
                table: "Venda",
                column: "VeiculoId",
                principalTable: "Veiculo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
