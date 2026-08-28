using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Products.Chat.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class Stage2bKnowledgeAdrWorkItemArtifact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "knowledge");

            migrationBuilder.EnsureSchema(
                name: "work");

            migrationBuilder.CreateTable(
                name: "Knowledge",
                schema: "knowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('KNW-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knowledge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkItem",
                schema: "work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('WRK-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adr",
                schema: "knowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KnowledgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('ADR-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adr_Knowledge_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalSchema: "knowledge",
                        principalTable: "Knowledge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artifact",
                schema: "work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Ref = table.Column<string>(type: "nvarchar(450)", nullable: false, computedColumnSql: "('ART-' + RIGHT('00000000' + CAST([Seq] AS varchar(8)), 8))", stored: true),
                    Seq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artifact_WorkItem_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: "work",
                        principalTable: "WorkItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adr_KnowledgeId",
                schema: "knowledge",
                table: "Adr",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "UQ_Adr_Ref",
                schema: "knowledge",
                table: "Adr",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artifact_WorkItemId",
                schema: "work",
                table: "Artifact",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "UQ_Artifact_Ref",
                schema: "work",
                table: "Artifact",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Knowledge_WorkspaceId_Status",
                schema: "knowledge",
                table: "Knowledge",
                columns: new[] { "WorkspaceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "UQ_Knowledge_Ref",
                schema: "knowledge",
                table: "Knowledge",
                column: "Ref",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_ProjectId_Status",
                schema: "work",
                table: "WorkItem",
                columns: new[] { "ProjectId", "Status" });

            migrationBuilder.CreateIndex(
                name: "UQ_WorkItem_Ref",
                schema: "work",
                table: "WorkItem",
                column: "Ref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adr",
                schema: "knowledge");

            migrationBuilder.DropTable(
                name: "Artifact",
                schema: "work");

            migrationBuilder.DropTable(
                name: "Knowledge",
                schema: "knowledge");

            migrationBuilder.DropTable(
                name: "WorkItem",
                schema: "work");
        }
    }
}
