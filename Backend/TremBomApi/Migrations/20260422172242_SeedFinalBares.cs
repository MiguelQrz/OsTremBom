using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TremBomApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedFinalBares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "locais",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "categoria", "descricao" },
                values: new object[] { "Edifício Gastronômico", "O clássico: fígado com jiló, queijos premiados e o melhor doce de leite." });

            migrationBuilder.InsertData(
                table: "locais",
                columns: new[] { "id", "ativo", "categoria", "data_cadastro", "descricao", "imagem_url", "nome", "total_comentarios", "total_likes" },
                values: new object[,]
                {
                    { 2, true, "Edifício Gastronômico", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cozinha de raiz, bares artesanais e um ambiente retrô industrial incrível.", null, "Mercado Novo", 0, 0 },
                    { 3, true, "Edifício Gastronômico/Bar", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varandas icônicas e bares históricos no centro de BH.", null, "Edifício Maletta", 0, 0 },
                    { 4, true, "Bar Tradicional", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "O bar mais antigo de BH, patrimônio do bairro Santa Tereza.", null, "Bar do Orlando", 0, 0 },
                    { 5, true, "Bar Artesanal", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cervejaria local com clima de calçada e muita mineiridade.", null, "Juramento 202", 0, 0 },
                    { 6, true, "Bar/Cultura", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gastronomia e drinks com a melhor vista para os murais do CURA.", null, "Casa Sapucaí", 0, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "locais",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "categoria", "descricao" },
                values: new object[] { "Edificio Gastronomico", "O clássico: fígado com jiló, queijos premiados e o melhor doce de leite" });
        }
    }
}
