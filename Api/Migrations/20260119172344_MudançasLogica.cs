using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class MudançasLogica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "system_resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    exhibition_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "access_permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    system_resource_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_permissions_system_resources_system_resource_id",
                        column: x => x.system_resource_id,
                        principalTable: "system_resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_access_permissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "convenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero_convenio = table.Column<string>(type: "text", nullable: false),
                    titulo = table.Column<string>(type: "text", nullable: false),
                    descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    orgao_concedente = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    data_publicacao_diario = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data_vigencia_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data_vigencia_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_convenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_convenio_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "system_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    used_payload = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_system_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "documentos_convenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    convenio_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_documento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nome_arquivo_original = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    nome_arquivo_salvo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    caminho_arquivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tamanho_bytes = table.Column<long>(type: "bigint", nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    uploaded_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentos_convenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_documentos_convenio_convenio_convenio_id",
                        column: x => x.convenio_id,
                        principalTable: "convenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_documentos_convenio_users_uploaded_by_user_id",
                        column: x => x.uploaded_by_user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_permissions_system_resource_id",
                table: "access_permissions",
                column: "system_resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_permissions_user_id_system_resource_id",
                table: "access_permissions",
                columns: new[] { "user_id", "system_resource_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_convenio_created_by_user_id",
                table: "convenio",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_convenio_numero_convenio",
                table: "convenio",
                column: "numero_convenio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documentos_convenio_convenio_id",
                table: "documentos_convenio",
                column: "convenio_id");

            migrationBuilder.CreateIndex(
                name: "IX_documentos_convenio_uploaded_by_user_id",
                table: "documentos_convenio",
                column: "uploaded_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_logs_user_id",
                table: "system_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_resources_name",
                table: "system_resources",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_permissions");

            migrationBuilder.DropTable(
                name: "documentos_convenio");

            migrationBuilder.DropTable(
                name: "system_logs");

            migrationBuilder.DropTable(
                name: "system_resources");

            migrationBuilder.DropTable(
                name: "convenio");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
