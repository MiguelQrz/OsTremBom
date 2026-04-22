using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TremBomApi.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoNomesESeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 1);

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

            migrationBuilder.InsertData(
                table: "locais",
                columns: new[] { "id", "ativo", "categoria", "data_cadastro", "descricao", "imagem_url", "nome", "total_comentarios", "total_likes" },
                values: new object[,]
                {
                    { 101, true, "Edifício Gastronômico", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "O clássico: fígado com jiló, queijos premiados e o melhor doce de leite.", "https://viajenaweb.com/wp-content/uploads/2016/12/O-que-fazer-no-Mercado-Central-de-Belo-Horizonte-768x432.jpg.webp", "Mercado Central", 0, 0 },
                    { 102, true, "Edifício Gastronômico", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cozinha de raiz, bares artesanais e um ambiente retrô industrial incrível.", "https://www.hojeemdia.com.br/image/policy:1.992457.1702919772:1702919772/image.jpg?f=2x1&w=1200", "Mercado Novo", 0, 0 },
                    { 103, true, "Edifício Gastronômico/Bar", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varandas icônicas e bares históricos no centro de BH.", "https://resize.casapino.com.br/?u=https://cms-bomgourmet.s3.amazonaws.com/bomgourmet/2018/10/201810/maletta-belo-horizonte-varanda-20f288a0.jpg&w=661", "Edifício Maletta", 0, 0 },
                    { 104, true, "Bar", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "O bar mais antigo de BH, patrimônio do bairro Santa Tereza.", "https://folhadesetelagoas.com/images/noticias/190/9f40c1539471322fcf360b2a9be33a36.jpeg", "Bar do Orlando", 0, 0 },
                    { 105, true, "Bar Artesanal", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cervejaria local com clima de calçada e muita mineiridade.", "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/15/04/71/ad/img-20181005-wa0018-largejpg.jpg?w=1000&h=-1&s=1", "Juramento 202", 0, 0 },
                    { 106, true, "Pizarria", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "é uma pizzaria badalada e charmosa, localizada em um casarão dos anos 30 no bairro Carlos Prates, em Belo Horizonte, com vista panorâmica da cidade e ambiente de praça", "https://andadireito.com.br/wp-content/uploads/2025/12/Forno-da-Saudade-5.png", "Forno da Saudade", 0, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "locais",
                keyColumn: "id",
                keyValue: 106);

            migrationBuilder.InsertData(
                table: "locais",
                columns: new[] { "id", "ativo", "categoria", "data_cadastro", "descricao", "imagem_url", "nome", "total_comentarios", "total_likes" },
                values: new object[,]
                {
                    { 1, true, "Edifício Gastronômico", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "O clássico: fígado com jiló, queijos premiados e o melhor doce de leite.", "https://viajenaweb.com/wp-content/uploads/2016/12/O-que-fazer-no-Mercado-Central-de-Belo-Horizonte-768x432.jpg.webp", "Mercado Central", 0, 0 },
                    { 2, true, "Edifício Gastronômico", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cozinha de raiz, bares artesanais e um ambiente retrô industrial incrível.", null, "Mercado Novo", 0, 0 },
                    { 3, true, "Edifício Gastronômico/Bar", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varandas icônicas e bares históricos no centro de BH.", null, "Edifício Maletta", 0, 0 },
                    { 4, true, "Bar Tradicional", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "O bar mais antigo de BH, patrimônio do bairro Santa Tereza.", null, "Bar do Orlando", 0, 0 },
                    { 5, true, "Bar Artesanal", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cervejaria local com clima de calçada e muita mineiridade.", null, "Juramento 202", 0, 0 },
                    { 6, true, "Bar/Cultura", new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gastronomia e drinks com a melhor vista para os murais do CURA.", null, "Casa Sapucaí", 0, 0 }
                });
        }
    }
}
