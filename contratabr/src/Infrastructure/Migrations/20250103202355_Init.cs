using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Origem = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Destino = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Valor = table.Column<double>(type: "double precision", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Route",
                columns: new[] { "Id", "DateUpdated", "Destino", "Origem", "Valor" },
                values: new object[,]
                {
                    { 1L, null, "BRC", "GRU", 10.0 },
                    { 2L, null, "SCL", "BRC", 5.0 },
                    { 3L, null, "CDG", "GRU", 75.0 },
                    { 4L, null, "SCL", "GRU", 20.0 },
                    { 5L, null, "ORL", "GRU", 56.0 },
                    { 6L, null, "CDG", "ORL", 5.0 },
                    { 7L, null, "ORL", "SCL", 20.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Route");
        }
    }
}
