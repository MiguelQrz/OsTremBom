using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TremBomApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locais",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    categoria = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    descricao = table.Column<string>(type: "TEXT", nullable: true),
                    imagem_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    total_likes = table.Column<int>(type: "INTEGER", nullable: false),
                    total_comentarios = table.Column<int>(type: "INTEGER", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locais", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome_completo = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    senha_hash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    foto_perfil_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    data_cadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    termos_aceitos_em = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ultimo_login = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publicacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    usuario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    local_id = table.Column<int>(type: "INTEGER", nullable: false),
                    local_nome = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    feedback = table.Column<string>(type: "TEXT", nullable: true),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    total_likes = table.Column<int>(type: "INTEGER", nullable: false),
                    total_comentarios = table.Column<int>(type: "INTEGER", nullable: false),
                    total_compartilhamentos = table.Column<int>(type: "INTEGER", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicacoes_locais_local_id",
                        column: x => x.local_id,
                        principalTable: "locais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_publicacoes_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    usuario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    token = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    token_lembrar = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    data_expiracao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessoes_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_preferencias",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    usuario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    preferencia = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_preferencias", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_preferencias_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comentarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    publicacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    usuario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    texto = table.Column<string>(type: "TEXT", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_comentarios_publicacoes_publicacao_id",
                        column: x => x.publicacao_id,
                        principalTable: "publicacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comentarios_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicacoes_fotos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    publicacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    foto_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ordem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicacoes_fotos", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicacoes_fotos_publicacoes_publicacao_id",
                        column: x => x.publicacao_id,
                        principalTable: "publicacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicacoes_likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    publicacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    usuario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    data_like = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicacoes_likes", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicacoes_likes_publicacoes_publicacao_id",
                        column: x => x.publicacao_id,
                        principalTable: "publicacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_publicacoes_likes_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_publicacao_id",
                table: "comentarios",
                column: "publicacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_usuario_id",
                table: "comentarios",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacoes_local_id",
                table: "publicacoes",
                column: "local_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacoes_usuario_id",
                table: "publicacoes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacoes_fotos_publicacao_id",
                table: "publicacoes_fotos",
                column: "publicacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacoes_likes_publicacao_id",
                table: "publicacoes_likes",
                column: "publicacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_publicacoes_likes_usuario_id",
                table: "publicacoes_likes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_sessoes_usuario_id",
                table: "sessoes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_preferencias_usuario_id",
                table: "usuarios_preferencias",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comentarios");

            migrationBuilder.DropTable(
                name: "publicacoes_fotos");

            migrationBuilder.DropTable(
                name: "publicacoes_likes");

            migrationBuilder.DropTable(
                name: "sessoes");

            migrationBuilder.DropTable(
                name: "usuarios_preferencias");

            migrationBuilder.DropTable(
                name: "publicacoes");

            migrationBuilder.DropTable(
                name: "locais");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
