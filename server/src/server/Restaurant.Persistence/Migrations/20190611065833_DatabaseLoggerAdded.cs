using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Restaurant.Persistence.Migrations
{
    public partial class DatabaseLoggerAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LogLevel = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    QueryString = table.Column<string>(nullable: true),
                    ExceptionType = table.Column<string>(nullable: true),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    HttpMethod = table.Column<string>(nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
