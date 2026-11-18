using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PontosApoioApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_PONTOS_APOIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_PONTO_APOIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NM_ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NM_BAIRRO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NM_TELEFONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NR_CAPACIDADE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ST_ATIVO = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_PONTOS_APOIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBL_RECURSOS_ENERGIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_TIPO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NR_QUANTIDADE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NR_AUTONOMIA_HORAS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_PONTO_APOIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_RECURSOS_ENERGIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBL_RECURSOS_ENERGIA_TBL_PONTOS_APOIO_ID_PONTO_APOIO",
                        column: x => x.ID_PONTO_APOIO,
                        principalTable: "TBL_PONTOS_APOIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBL_RECURSOS_ENERGIA_ID_PONTO_APOIO",
                table: "TBL_RECURSOS_ENERGIA",
                column: "ID_PONTO_APOIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_RECURSOS_ENERGIA");

            migrationBuilder.DropTable(
                name: "TBL_PONTOS_APOIO");
        }
    }
}
