using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Products.Chat.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "org");

            migrationBuilder.CreateTable(
                name: "Workspace",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('WKS-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Workspace_Ref",
                schema: "org",
                table: "Workspace",
                column: "Ref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workspace",
                schema: "org");
        }
    }
}
